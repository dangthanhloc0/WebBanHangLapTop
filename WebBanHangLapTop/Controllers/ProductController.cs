using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebBanHangLapTop.Models;
using WebBanHangLapTop.Repository;

namespace WebBanHangLapTop.Controllers
{
	public class ProductController : Controller
	{

		//URL : Product/Add

		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;
		public ProductController(IProductRepository productRepository,
		ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}
		// Hiển thị danh sách sản phẩm

	
		public async Task< IActionResult> Index()
		{
			var product1 = await _productRepository.GetAllAsync();
			ViewBag.categorys= await _categoryRepository.GetAllAsync();	
			return View(product1);
		}
		// Hiển thị form thêm sản phẩm mới
	
	
		// Hiển thị thông tin chi tiết sản phẩm


		public async Task<IActionResult> Display(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
		
			if (product == null)
			{
				return NotFound();
			}

			ViewBag.images = await _productRepository.GetAllImagesById(id);

			return View(product);
		}
		// Hiển thị form cập nhật sản phẩm
	
		// Xử lý cập nhật sản phẩm
	
	
		// Hiển thị form xác nhận xóa sản phẩm
	
		// Xử lý xóa sản phẩm
		[HttpPost]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			await _productRepository.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}


		private async Task<string> SaveImage(IFormFile image)
		{
			var savePath = Path.Combine("wwwroot/Image", image.FileName);
			using(var fileStream = new FileStream(savePath, FileMode.Create))
			{
				await image.CopyToAsync(fileStream);
			}
			return "/Image/" + image.FileName;
		}

    }
}