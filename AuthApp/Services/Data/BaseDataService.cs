using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace AuthApp.Services.Data
{
    public abstract class BaseDataService
    {
        bool _showLoading;
        string _loadingMessage;

        private static readonly Lazy<HttpClient> _lazyInstance = new Lazy<HttpClient>(() => new HttpClient());

        public static HttpClient HttpClient
        {
            get
            {
                return _lazyInstance.Value;
            }
        }

        public BaseDataService SetBearerToken(string token)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return this;
        }

        /// <summary>
        /// Sets the base address.
        /// </summary>
        /// <returns>The base address.</returns>
        /// <param name="baseAddress">Base rest address. *MUST end with a trailing slash*</param>
        public BaseDataService SetBaseAddress(string baseAddress = "http://localhost:7071/api/")
        {
            HttpClient.BaseAddress = new Uri(baseAddress);
            return this;
        }

        public BaseDataService ShowLoadingMessage(bool showLoading, string message = "Loading")
        {
            _showLoading = showLoading;
            _loadingMessage = message;
            return this;
        }

        protected async Task<T> GetDataAsync<T>(string endpoint)
        {
            if (_showLoading)
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading(_loadingMessage);

            var response = await HttpClient.GetAsync(endpoint);

            if(_showLoading)
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();

            if(response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(data);
            }
            else
            {

                return default(T);
            }

        }
    }
}
