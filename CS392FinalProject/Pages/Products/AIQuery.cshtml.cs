using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS392FinalProject.Services;
using CS392FinalProject.Models;
using System.Text.Json;

namespace CS392FinalProject.Pages.Products
{
    public class AiQueryModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly GeminiService _gemini;

        public AiQueryModel(ProductService productService, GeminiService gemini)
        {
            _productService = productService;
            _gemini = gemini;
        }

        [BindProperty]
        public string UserQuery { get; set; } = "";

        public string? AiAnswer { get; set; }
        public List<Product> MatchedProducts { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Load all products from MongoDB
            var products = await _productService.GetAllAsync();

            // 2. Build strict JSON-only prompt
            var prompt = $@"
You are an AI assistant for a computer store.

RULES:
- You MUST answer ONLY using the product list provided.
- If the user asks about anything not in the list, respond with:
  ""I can only answer questions about products in the database.""
- Your ENTIRE response MUST be ONLY valid JSON.
- DO NOT include markdown.
- DO NOT include code fences.
- DO NOT include explanations.
- DO NOT include commentary.
- DO NOT include text before or after the JSON.

User question:
""{UserQuery}""

Product list (ID | Name | Category | Price | InStock):
{string.Join("\n", products.Select(p => $"{p.Id} | {p.Name} | {p.Category} | {p.Price} | {p.InStock}"))}

Return ONLY this JSON structure:

{{
  ""matchedProductIds"": [""id1"", ""id2""],
  ""answer"": ""Natural language summary of the results.""
}}
";

            // 3. Call Gemini
            var aiText = await _gemini.AskGeminiAsync(prompt);

            // 4. Clean JSON (remove ```json or ``` wrappers)
            aiText = aiText.Trim();
            if (aiText.StartsWith("```"))
            {
                aiText = aiText
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();
            }

            // 5. Try to parse JSON
            GeminiInterpretationResult? result = null;

            try
            {
                result = JsonSerializer.Deserialize<GeminiInterpretationResult>(
                    aiText,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch
            {
                // Log raw AI output for debugging
                System.IO.File.WriteAllText("last_ai_response.txt", aiText);

                AiAnswer = "AI returned an invalid response. Check last_ai_response.txt for details.";
                return Page();
            }

            // 6. Display AI answer
            AiAnswer = result?.Answer ?? "No answer returned.";

            // 7. Filter MongoDB results by IDs returned by Gemini
            if (result?.MatchedProductIds != null)
            {
                MatchedProducts = products
                    .Where(p => result.MatchedProductIds.Contains(p.Id))
                    .ToList();
            }

            return Page();
        }
    }

    public class GeminiInterpretationResult
    {
        public List<string> MatchedProductIds { get; set; } = new();
        public string Answer { get; set; } = "";
    }
}
