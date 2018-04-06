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

        public BaseDataService SetHeaderValue(string name, string value)
        {
            HttpClient.DefaultRequestHeaders.Remove(name);

            HttpClient.DefaultRequestHeaders.Add(name, value);

            return this;
        }

        protected async Task<T> GetDataAsync<T>(string endpoint)
        {
            if(!Plugin.Connectivity.CrossConnectivity.IsSupported || !Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast("No connectivity", TimeSpan.FromSeconds(3));
                return default(T);
            }

            if (_showLoading)
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading(_loadingMessage);

            HttpResponseMessage response = null;

            try
            {
                response = await HttpClient.GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                Acr.UserDialogs.UserDialogs.Instance.Toast(ex.Message, TimeSpan.FromSeconds(3));
            }

            if(_showLoading)
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();

            if(response != null && response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                T result = default(T);

                try
                {
                    result = JsonConvert.DeserializeObject<T>(data);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }

                return result;
            }
            else
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast("Unable to get data", TimeSpan.FromSeconds(3));
                return default(T);
            }

        }

        protected async Task<string> GetDataAsStringAsync(string endpoint)
        {
            if (!Plugin.Connectivity.CrossConnectivity.IsSupported || !Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast("No connectivity", TimeSpan.FromSeconds(3));
                return string.Empty;
            }

            if (_showLoading)
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading(_loadingMessage);

            HttpResponseMessage response = null;

            try
            {
                response = await HttpClient.GetAsync(endpoint);
                System.Diagnostics.Debug.WriteLine(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                Acr.UserDialogs.UserDialogs.Instance.Toast(ex.Message, TimeSpan.FromSeconds(3));
            }

            if (_showLoading)
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();

            if (response != null && response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                return data;
            }
            else if(response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast("Unable to get data", TimeSpan.FromSeconds(3));
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
