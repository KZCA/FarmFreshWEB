using FarmFreshWEB.Helpers;
using FarmFreshWEB.Models;
using FarmFreshWEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmFreshWEB.Controllers
{
	public class CartController : Controller
	{
		private readonly IProductService _productService;
		public CartController(IProductService productService)
		{
			_productService = productService;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson < List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			CartViewModel cartVM = new()
			{
				CartItems = cart,
				GrandTotal = cart.Sum(x => x.Quantity * x.Price)
			};

			return View(cartVM);
		}

		public async Task<IActionResult> Add(int id)
		{
			ProductModel product = await _productService.GetProductById(id);

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			CartItemModel cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

			if(cartItem == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItem.Quantity += 1;
			}

			HttpContext.Session.SetJson("Cart", cart);

			TempData["Success"] = "The product has been added!";

			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Decrease(int id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			CartItemModel cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == id);
			}

			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["Success"] = "The product has been removed!";

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(int id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			cart.RemoveAll(p => p.ProductId == id);

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["Success"] = "The product has been removed!";

			return RedirectToAction("Index");
		}

		public IActionResult Clear()
		{
			HttpContext.Session.Remove("Cart");

			return RedirectToAction("Index");
		}
	}
}
