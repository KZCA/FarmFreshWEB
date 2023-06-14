using FarmFreshWEB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFreshWEB.Services
{
	public interface IGetToken
	{
		Task<string> RequestToken();
	}
	
	public class GetToken:IGetToken
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _config;
        public GetToken(IHttpClientFactory httpClientFactory,IConfiguration config)
        {
			_httpClientFactory = httpClientFactory;
			_config = config;
        }

		public async Task<string> RequestToken()
		{
			string token = string.Empty;

			var httpClient = _httpClientFactory.CreateClient();

			StringContent content = new StringContent(JsonConvert.SerializeObject(new
			{
				name = _config.GetValue<string>("URL:Username"),
				password = _config.GetValue<string>("URL:Password")
			}), Encoding.UTF8, "application/json");

			HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Post, Content = content, RequestUri = new Uri(_config.GetValue<string>("URL:Token_URL")) };
			
			HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

			string apiresponse = await response.Content.ReadAsStringAsync();
			var obj = JsonConvert.DeserializeObject<TokenModel>(apiresponse);
			token = obj.Access_Token;		

			return token;
		}
    }
}
