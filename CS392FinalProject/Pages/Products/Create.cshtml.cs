using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS392FinalProject.Services;
using CS392FinalProject.Models;

namespace CS392FinalProject.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly ProductService _service;

        public CreateModel(ProductService service)
        {
            _service = service;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _service.CreateAsync(Product);
            return RedirectToPage("Index");
        }
    }
}
