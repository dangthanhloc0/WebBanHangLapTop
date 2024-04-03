﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanHangLapTop.Models;
using WebBanHangLapTop.Repository;

namespace WebBanHangLapTop.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;


       
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, IProductRepository
        productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

   
        public async Task<IActionResult> AddToCart(int Id, int quantity)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var product = await _productRepository.GetByIdAsync(Id);
            var cartItem = new CartItem
            {
                ProductId = Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            };
            var cart =
            HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new
            ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var cart =
            HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new
            ShoppingCart();
            return View(cart);
        }
        // Các actions khác...


        public IActionResult Checkout()
        {
            return View(new Order());
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart =
            HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);

                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
       


        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart =
            HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index");
            }
            var user = await _userManager.GetUserAsync(User);
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted", order.Id); 
}
    }

}


