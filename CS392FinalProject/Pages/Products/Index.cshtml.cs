using Microsoft.AspNetCore.Mvc.RazorPages;
using CS392FinalProject.Services;
using CS392FinalProject.Models;

namespace CS392FinalProject.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductService _service;

        public IndexModel(ProductService service)
        {
            _service = service;
        }





        public List<Product> Products { get; set; } = new();


        public async Task OnGetAsync()
        {
            Products = await _service.GetAllAsync();
        }
    }
}
