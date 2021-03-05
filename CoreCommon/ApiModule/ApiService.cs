using CoreCommon.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseApi<T>> getAsync<T>(RequestApi api) where T : class
        {

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, api.apiUrl);

            // request.Headers.Add("Content-Type", "application/json");
            if (api.isAuth)
            {
                request.Headers.Authorization = api.auth;
            }

            var response = await client.SendAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new ResponseApi<T> { code = response.StatusCode, message = "调用失败", data = null };

            }

            var respmessg = await response.Content.ReadAsStringAsync();
            if (typeof(T) == typeof(string))
            {
                return new ResponseApi<T> { code = response.StatusCode, data = respmessg as T };
            }
            else
            {

                return new ResponseApi<T> { code = response.StatusCode, data = JsonConvert.DeserializeObject<T>(respmessg) };
            }




        }

        public async Task<ResponseApi<T>> postAsync<T>(RequestApi api) where T : class
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, api.apiUrl);

            if (api.isAuth)
            {
                request.Headers.Authorization = api.auth;
            }

            if (api.data != null)
            {

                string Body = JsonConvert.SerializeObject(api.data);
                request.Content = new StringContent(Body, Encoding.UTF8, "application/json");

            }
            var response = await client.SendAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new ResponseApi<T> { code = response.StatusCode, message = "调用失败", data = null };

            }

            var respmessg = await response.Content.ReadAsStringAsync();
            return new ResponseApi<T> { code = response.StatusCode, data = JsonConvert.DeserializeObject<T>(respmessg) };
        }
    }
}
