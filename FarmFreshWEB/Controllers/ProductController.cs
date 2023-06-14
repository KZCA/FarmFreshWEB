using FarmFreshWEB.Models;
using FarmFreshWEB.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FarmFreshWEB.Controllers
{
	public class ProductController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;
        public ProductController(ICategoryService categoryService,IProductService productService)
        {
            _categoryService = categoryService;
			_productService = productService;
        }
        public async Task<IActionResult> Index(string categorySlug="",int page = 1, int pageSize = 3) 
		{
			IEnumerable<ProductModel> _products = new List<ProductModel>();

			ViewBag.CategorySlug = categorySlug;
			ViewBag.PageNumber = page;
			ViewBag.PageRange = pageSize;

			if (string.IsNullOrEmpty(categorySlug))
			{
				var response = await _productService.PaginationProductList(1, 5);
				_products = response.data;
				ViewBag.TotalPage = response.totalPages;
				return View(_products.OrderByDescending(p=>p.Id).ToList());
			}

			CategoryModel category = await _categoryService.GetCategoryBySlug(categorySlug);
			if (category == null) return RedirectToAction("Index");

			var productbyCategory =await _productService.GetAllProductByCategoryId(Convert.ToInt16(category.Id),page, pageSize);
			_products = productbyCategory.data == null? new List<ProductModel>(): productbyCategory.data;
			ViewBag.TotalPage = productbyCategory.totalPages;
			return View(_products.OrderByDescending(p => p.Id).ToList());
		}
	}
}
