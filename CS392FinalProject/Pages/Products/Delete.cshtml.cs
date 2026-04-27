using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS392FinalProject.Services;
using CS392FinalProject.Models;

namespace CS392FinalProject.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly ProductService _service;

        public DeleteModel(ProductService service)
        {
            _service = service;
        }

        public Product Product { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            Product = product;
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            await _service.DeleteAsync(id);

            return RedirectToPage("Index");
        }
    }
}
