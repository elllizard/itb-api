using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using itb_api.Models.Configurtations;
using itb_api.Models.Instagram;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace itb_api.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        private readonly ILogger<InstagramService> _logger;
        private readonly InstagramConfiguration _instagramConfig;
        private readonly CookieContainer _cookieContainer;

        public InstagramService(
            ILogger<InstagramService> logger,
            IOptions<InstagramConfiguration> instagramConfig
        )
        {
            _logger = logger;
            _instagramConfig = instagramConfig.Value;
            _cookieContainer = new CookieContainer();

            _applyCookies();
        }

        public async Task<GetUserProfileByUsernameResponse> GetUserProfileByUsernameAsync(string username)
        {
            StringBuilder _url = new StringBuilder();
            _url.Append(_instagramConfig.Url);
            _url.Replace("[base]", _instagramConfig.BaseUrl);
            _url.Replace("[username]", username);

            using HttpClientHandler _httpClientHandler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer
            };

            using HttpClient _httpClient = new HttpClient(_httpClientHandler);

            HttpResponseMessage _result = await _httpClient.GetAsync(_url.ToString());
            string _response = await _result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<GetUserProfileByUsernameResponse>(_response);
        }

        private void _applyCookies()
        {
            string[] _cookies = _instagramConfig.Cookies.Split(";");
            
            foreach (string _cookieRow in _cookies)
            {
                string[] _cookie = _cookieRow.Split("=");
                
                _cookieContainer.Add(new Cookie(_cookie[0], _cookie[1], "/", ".instagram.com"));
            }
        }
    }
}