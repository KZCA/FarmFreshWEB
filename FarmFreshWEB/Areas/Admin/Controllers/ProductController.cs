using FarmFreshWEB.Models;
using FarmFreshWEB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Eventing.Reader;

namespace FarmFreshWEB.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(ICategoryService categoryService, IProductService productService,IWebHostEnvironment webHost)
		{
			_categoryService = categoryService;
			_productService = productService;
			_webHostEnvironment = webHost;
		}
		public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
		{
			IEnumerable<ProductModel> _products = new List<ProductModel>();
			
			ViewBag.PageNumber = page;
			ViewBag.PageRange = pageSize;

			var response = await _productService.GeAllProductByIncudeCategory(1, 5);
			_products = response.data;
			ViewBag.TotalPage = response.totalPages;
			return View(_products.OrderByDescending(p => p.Id).ToList());
			
		}

        public async Task<IActionResult> Create()
        {
			List<CategoryModel> categories = new List<CategoryModel>();
			categories = (await _categoryService.GetCategories()).ToList();
			
			ViewBag.Categories = new SelectList(categories, "Id","Name");
			return View();
        }

		[HttpPost]
        public async Task<IActionResult> Create(ProductModel product)
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            categories = (await _categoryService.GetCategories()).ToList();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

			if(ModelState.IsValid)
			{
				product.Slug = product.Name.ToLower().Replace(" ", "_");
				var slug = await _productService.GetProductBySlug(product.Slug);
				if(slug.Name != null)
				{
					ModelState.AddModelError("", "The product already exists.");
					return View(product);
				}

				string imageName;
				if(product.ImageUpload != null)
				{
					string uplaodDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/screen3");
					imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;

					string filePath = Path.Combine(uplaodDir,imageName);

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();

					product.Image = imageName;
				}

				await _productService.CreateProdcut(product);

                TempData["Success"] = "The product has been created!";

				return RedirectToAction("Index");

            }
			return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            categories = (await _categoryService.GetCategories()).ToList();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

			ProductModel product = await _productService.GetProductById(id);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductModel product)
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            categories = (await _categoryService.GetCategories()).ToList();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "_");
                var slug = await _productService.GetProductBySlug(product.Slug);
                if (slug.Name != null)
                {
                    ModelState.AddModelError("", "The product already exists.");
                    return View(product);
                }

                string imageName;
                if (product.ImageUpload != null)
                {
                    string uplaodDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/screen3");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;

                    string filePath = Path.Combine(uplaodDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    product.Image = imageName;
                }              

            }

            await _productService.UpdateProduct(product);

            TempData["Success"] = "The product has been updated!";
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            categories = (await _categoryService.GetCategories()).ToList();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");  

            ProductModel product = await _productService.GetProductById(id);

            if (!string.IsNullOrEmpty(product.Image))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/screen3");
                string oldImagePath = Path.Combine(uploadDir,product.Image);
                if(System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            await _productService.DeleteProduct(Convert.ToInt16(product.Id));
            TempData["Success"] = "The product has been deleted!";
            return RedirectToAction("Index");
        }
    }
}
