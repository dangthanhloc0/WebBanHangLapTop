using Microsoft.AspNetCore.Mvc;
using WebBanHangLapTop.Models;
using WebBanHangLapTop.Repository;

namespace WebBanHangLapTop.Controllers
{
    public class CategoryController : Controller
    {

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        // 
   
   

        public async Task<IActionResult> Category()
        {
            ViewBag.ListNumberCategori = await _productRepository.GetListCountCategory();
            var category = await _categoryRepository.GetAllAsync();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    }
}
