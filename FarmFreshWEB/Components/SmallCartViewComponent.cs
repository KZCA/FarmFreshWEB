using FarmFreshWEB.Helpers;
using FarmFreshWEB.Models;
using FarmFreshWEB.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;

namespace FarmFreshWEB.Components
{
	public class SmallCartViewComponent : ViewComponent
	{		
		public IViewComponentResult Invoke()
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			SmallCartViewModel smallCartVM;

			if(cart == null || cart.Count == 0)
			{
				smallCartVM = null;
			}
			else
			{
				smallCartVM = new()
				{
					NumberOfItems = cart.Sum(x => x.Quantity),
					TotalAmount = cart.Sum(x => x.Quantity * x.Price)
				};
			}
			return View(smallCartVM);
		}
		
			

    }
}
