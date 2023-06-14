using FarmFreshWEB.Models;
using FarmFreshWEB.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;

namespace FarmFreshWEB.Components
{
	public class CategoryViewComponent:ViewComponent
	{		
		private readonly IConfiguration _config;
		private readonly ICategoryService _categoryService;

		CategoryModel _category = new CategoryModel();
		public CategoryViewComponent(IConfiguration config, ICategoryService categoryService)
        {			
			_config = config;
			_categoryService = categoryService;
        }

		public async Task<IViewComponentResult> InvokeAsync() => View(await _categoryService.GetCategories());
		
			

    }
}
