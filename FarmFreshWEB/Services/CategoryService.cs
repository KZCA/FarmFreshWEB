using FarmFreshWEB.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FarmFreshWEB.Services
{	
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryModel>> GetCategories();
		Task<CategoryModel> GetCategoryBySlug(string slug);
	}
	public class CategoryService:ICategoryService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _config;
		private readonly IGetToken _getToken;
		public CategoryService(IHttpClientFactory httpClientFactory,IConfiguration config, IGetToken getToken)
		{
			_httpClientFactory = httpClientFactory;
			_config = config;
			_getToken = getToken;
		}

		public async Task<IEnumerable<CategoryModel>> GetCategories()
		{
			IEnumerable<CategoryModel> _categorylist = new List<CategoryModel>();

			var httpClient = _httpClientFactory.CreateClient();

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Category") };
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());

			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			string apiresponse = await response.Content.ReadAsStringAsync();
			_categorylist = JsonConvert.DeserializeObject<List<CategoryModel>>(apiresponse);
			return _categorylist;			
		}

		public async Task<CategoryModel> GetCategoryBySlug(string slug)
		{
			CategoryModel _category = new CategoryModel();

			var httpClient = _httpClientFactory.CreateClient();

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Category/" + slug) };
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());

			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			string apiresponse = await response.Content.ReadAsStringAsync();
			_category = JsonConvert.DeserializeObject<CategoryModel>(apiresponse);
			return _category;
			
		}
	}
}
