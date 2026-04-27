using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS392FinalProject.Services;
using CS392FinalProject.Models;

namespace CS392FinalProject.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly ProductService _service;

        public EditModel(ProductService service)
        {
            _service = service;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _service.UpdateAsync(Product.Id!, Product);
            return RedirectToPage("Index");
        }
    }
}
