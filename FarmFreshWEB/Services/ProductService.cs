using FarmFreshWEB.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace FarmFreshWEB.Services
{
	public interface IProductService
	{
		Task<ProductPaginationModel> PaginationProductList(int page, int pageSize);
		Task<ProductPaginationModel> GetAllProductByCategoryId(int categoryid, int page, int pagesize);
		Task<ProductModel> GetProductById(int id);
		Task<ProductPaginationModel> GeAllProductByIncudeCategory(int page, int pagesize);
		Task<ProductModel> GetProductBySlug(string slug);
		Task<bool> CreateProdcut(ProductModel model);
		Task<bool> UpdateProduct(ProductModel model);
        Task<bool> DeleteProduct(int id);

    }
	public class ProductService:IProductService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _config;
		private readonly IGetToken _getToken;
		public ProductService(IHttpClientFactory httpClientFactory,IConfiguration config, IGetToken getToken)
		{
			_httpClientFactory = httpClientFactory;
			_config = config;
			_getToken = getToken;
		}

		public async Task<ProductPaginationModel> PaginationProductList(int page, int pageSize)
		{
			ProductPaginationModel _pagination = new ProductPaginationModel();

			var httpClient = _httpClientFactory.CreateClient();

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Product/pagination/" + page + "/" + pageSize) };
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());

			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			string apiresponse = await response.Content.ReadAsStringAsync();
			_pagination = JsonConvert.DeserializeObject<ProductPaginationModel>(apiresponse);
			return _pagination;			
		}

		public async Task<ProductPaginationModel> GetAllProductByCategoryId(int categoryid, int page ,int pagesize)
		{
			ProductPaginationModel _pagination = new ProductPaginationModel();

			var httpClient = _httpClientFactory.CreateClient();

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Product/GetProductByCategoryId/" + categoryid + "/" + page + "/" + pagesize) };
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			string apiresponse = await response.Content.ReadAsStringAsync();
			_pagination = JsonConvert.DeserializeObject<ProductPaginationModel>(apiresponse);

			return _pagination;
			
		}

		public async Task<ProductModel> GetProductById(int id)
		{
			ProductModel _product = new ProductModel();

			var httpClient = _httpClientFactory.CreateClient();

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Product/" + id) };
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			if (response.IsSuccessStatusCode)
			{
                string apiresponse = await response.Content.ReadAsStringAsync();
                _product = JsonConvert.DeserializeObject<ProductModel>(apiresponse);
            }		

			return _product;

		}

        public async Task<ProductPaginationModel> GeAllProductByIncudeCategory(int page, int pagesize)
        {
            ProductPaginationModel _pagination = new ProductPaginationModel();

            var httpClient = _httpClientFactory.CreateClient();

            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Product/GetAllProductByIncludeCategory/" + page + "/" + pagesize) };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            string apiresponse = await response.Content.ReadAsStringAsync();
            _pagination = JsonConvert.DeserializeObject<ProductPaginationModel>(apiresponse);

            return _pagination;

        }

        public async Task<ProductModel> GetProductBySlug(string slug)
        {
            ProductModel _product = new ProductModel();

            var httpClient = _httpClientFactory.CreateClient();

            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Product/GetProductByslug/" + slug) };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			if (response.IsSuccessStatusCode)
			{
                string apiresponse = await response.Content.ReadAsStringAsync();
                _product = JsonConvert.DeserializeObject<ProductModel>(apiresponse);
            }          		

            return _product;

        }

        public async Task<bool> CreateProdcut(ProductModel model)
        {
            string token = string.Empty;

            var httpClient = _httpClientFactory.CreateClient();

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Post, Content = content, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL")+ "Product") };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			if (response.IsSuccessStatusCode)
				return true;

			else
				return false;
        }

        public async Task<bool> UpdateProduct(ProductModel model)
        {
            string token = string.Empty;

            var httpClient = _httpClientFactory.CreateClient();

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Put, Content = content, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Product") };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
                return true;

            else
                return false;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            string token = string.Empty;

            var httpClient = _httpClientFactory.CreateClient();            

            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Delete, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Product/" + id) };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
                return true;

            else
                return false;
        }
    }
}
