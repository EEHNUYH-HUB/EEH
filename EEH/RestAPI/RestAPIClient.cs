using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EEH.RestAPI
{
    public class RestAPIClient
    {

        private readonly TimeSpan timeout;
        private readonly DefaultContractResolver resolver;
        
        private const string MEDIATYPEJSON = "application/json";
        public string BaseUrl { get; set; }
        public Action<HttpRequestHeaders> OnHeaderSettingDelegate { get; set; }

        private static Dictionary<string, HttpClient> httpClients;


        public RestAPIClient(string baseUrl,
            DefaultContractResolver resolver = null
            ,
            TimeSpan? timeout = null)
        {
            this.resolver = resolver ?? new DefaultContractResolver();

            this.timeout = timeout ?? TimeSpan.FromSeconds(90);
            this.BaseUrl = baseUrl;
        }
        static readonly Object httpLocker = new Object();
        public HttpClient GetHttpClient()
        {
            lock (httpLocker)
            {
                if (httpClients == null)
                {
                    httpClients = new Dictionary<string, HttpClient>();
                }

                HttpClient client = null;

                if (!httpClients.ContainsKey(BaseUrl))
                {
                    client = new HttpClient();
                    client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }, false);
                    client.Timeout = timeout;
                    
                    


                    if (!string.IsNullOrEmpty(BaseUrl))
                        client.BaseAddress = new Uri(BaseUrl);
                    httpClients.Add(BaseUrl, client);
                }
                else
                {
                    client = httpClients[BaseUrl];
                }


                if (OnHeaderSettingDelegate != null)
                {
                    client.DefaultRequestHeaders.Clear();
                    OnHeaderSettingDelegate(client.DefaultRequestHeaders);

                }

                return client;
            }
        }


        public async Task<string> PutAsync(string url, HttpContent content)
        {
            HttpClient httpClient = GetHttpClient();


            using (var response = httpClient.PutAsync(url, content).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> GetAsync(string url)
        {
            HttpClient httpClient = GetHttpClient();

            using (var response = httpClient.GetAsync(url).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> DeleteAsync(string url)
        {
            HttpClient httpClient = GetHttpClient();
            using (var response = httpClient.DeleteAsync(url).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> PostAsync(string url, object input)
        {
            HttpClient httpClient = GetHttpClient();

            using (var requestContent = new StringContent(ConvertToJsonString(input), Encoding.UTF8, MEDIATYPEJSON))
            {
                using (var response = httpClient.PostAsync(url, requestContent).Result)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }


        }
        public async Task<string> PatchAsync(string url, object input)
        {
            HttpClient httpClient = GetHttpClient();


            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);

            request.Content = new StringContent(ConvertToJsonString(input), System.Text.Encoding.UTF8, MEDIATYPEJSON);

            using (var response = httpClient.SendAsync(request).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }


        }

        public async Task<T> PostAsync<T>(string url, object input)
        {
            string str = await PostAsync(url, input);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            string str = await GetAsync(url);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public async Task<T> DeleteAsync<T>(string url)
        {
            string str = await DeleteAsync(url);
            return JsonConvert.DeserializeObject<T>(str);
        }
        public async Task<T> PatchAsync<T>(string url, object input)
        {
            string str = await PatchAsync(url, input);
            return JsonConvert.DeserializeObject<T>(str);
        }


        private string ConvertToJsonString(object obj)
        {

            if (obj == null)
            {
                return string.Empty;
            }

            var rtn = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = resolver
            });

            return rtn;
        }

    }
}
