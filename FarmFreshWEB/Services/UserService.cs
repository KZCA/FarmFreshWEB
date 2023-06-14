using FarmFreshWEB.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace FarmFreshWEB.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserModel user);
        Task<bool> Login(LoginViewModel user);
		Task<bool> Logout();

	}
    public class UserService:IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly IGetToken _getToken;
        public UserService(IHttpClientFactory httpClientFactory, IConfiguration config, IGetToken getToken)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _getToken = getToken;
        }

        public async Task<bool> CreateUser(UserModel user)
        {
            string token = string.Empty; string errormessage = string.Empty;
            var httpClient = _httpClientFactory.CreateClient();

            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Post, Content = content, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Account/create") };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
                return true;

            else
                return false;
        }

		public async Task<bool> Login(LoginViewModel user)
		{
			string token = string.Empty; string errormessage = string.Empty;
			var httpClient = _httpClientFactory.CreateClient();

			StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Post, Content = content, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Account/login") };
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			if (response.IsSuccessStatusCode)
				return true;

			else
				return false;
		}

		public async Task<bool> Logout()
		{
			string token = string.Empty; string errormessage = string.Empty;
			var httpClient = _httpClientFactory.CreateClient();			

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = new Uri(_config.GetValue<string>("URL:Api_URL") + "Account/logout") };
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken.RequestToken());
			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
			if (response.IsSuccessStatusCode)
				return true;

			else
				return false;
		}
	}
}
