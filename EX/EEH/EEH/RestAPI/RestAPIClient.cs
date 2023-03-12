using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;//네임스페이스 선언(DefaultContractResolver 를 사용하기 위해 선언)
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using EEH;


namespace EEH.RestAPI
{
    public class RestAPIClient
    {

        private readonly TimeSpan timeout;
        private readonly DefaultContractResolver resolver;

        private const string MEDIATYPEJSON = "application/json";
        public string BaseUrl { get; set; }
        public delegate void HeaderDelegate(HttpRequestHeaders header);
        public HeaderDelegate OnHeaderSettingDelegate { get; set; }


        private static Dictionary<string, HttpClient> httpClients;


        public RestAPIClient(string baseUrl,
            DefaultContractResolver resolver = null,
            TimeSpan? timeout = null)  //2. <= 생성자 모든 클래스의 시작점 이라고 보면 됩니다
        {
            //Json Format 변경시 PascalCase형식으로 변경 됩니다
            //(PascalCase:첫단어를 대문자로 TestSample,CamelCase:첫글자는 소문자 두번째 단어부터 대문자로 표현testSample)
            //( API 주고 받는 데이터 형식이 JSON 형식 이고 주고받는 데이터 형식을 PascalCase 형식으로 
            this.resolver = resolver ?? new DefaultContractResolver();
            //Rest API 호출 타임아웃
            this.timeout = timeout ?? TimeSpan.FromSeconds(90);
            //호출 하고자 하는 Base URL
            this.BaseUrl = baseUrl;
        }
        /// <summary>
        /// 비동기 호출시 중복 호출 막기위해서
        /// </summary>
        static readonly Object httpLocker = new Object();
        /// <summary>
        /// Rest API 호출을 위한 HttpClient 인스턴스 생성
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            lock (httpLocker)
            {
                if (httpClients == null)
                {
                    httpClients = new Dictionary<string, HttpClient>();
                }

                HttpClient client = null;

                //HttpClient 인스턴스 생성 비용이 많고
                //인스턴스 유지 비용이 적게 사용하기때문에
                //제품이 끝나기 전까지 한번의 인스터스만 생성하게 로직
                if (!httpClients.ContainsKey(BaseUrl))
                {

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

        /// <summary>
        /// PUT 메서드 비동기 호츨
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> PutAsync(string url, HttpContent content)
        {
            HttpClient httpClient = GetHttpClient();


            using (var response = httpClient.PutAsync(url, content).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        /// <summary>
        /// GET 메서드 비동기 호출
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url)
        {
            HttpClient httpClient = GetHttpClient();

            using (var response = httpClient.GetAsync(url).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        /// <summary>
        /// DELETE 메서드 비동기 호출
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> DeleteAsync(string url)
        {
            HttpClient httpClient = GetHttpClient();
            using (var response = httpClient.DeleteAsync(url).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        /// <summary>
        /// POST 메서드 비동기 호출
        /// </summary>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, object input)
        {
            HttpClient httpClient = GetHttpClient();

            using (var requestContent = new StringContent(input.ExJsonSerializeString(resolver), Encoding.UTF8, MEDIATYPEJSON))
            {
                using (var response = httpClient.PostAsync(url, requestContent).Result)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }


        }

        /// <summary>
        /// Path 메서드 비동기 호출
        /// </summary>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> PatchAsync(string url, object input)
        {
            HttpClient httpClient = GetHttpClient();

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);


            request.Content = new StringContent(input.ExJsonSerializeString(resolver), System.Text.Encoding.UTF8, MEDIATYPEJSON);

            using (var response = httpClient.SendAsync(request).Result)
            {
                return await response.Content.ReadAsStringAsync();
            }


        }
        /// <summary>
        /// POST 메서드 비동기 호출
        /// T => 제네릭 클래스 외부에서 데이터 변수타입을 입력받아 내부에서 사용
        /// string(JSON) => T 형식으로 변환하여 리턴 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, object input)
        {
            string str = await PostAsync(url, input);
            return JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// GET 메서드 비동기 호출
        /// T => 제네릭 클래스 외부에서 데이터 변수타입을 입력받아 내부에서 사용
        /// string(JSON) => T 형식으로 변환하여 리턴 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url)
        {
            string str = await GetAsync(url);
            return JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// DELETE 메서드 비동기 호출
        /// T => 제네릭 클래스 외부에서 데이터 변수타입을 입력받아 내부에서 사용
        /// string(JSON) => T 형식으로 변환하여 리턴 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(string url)
        {
            string str = await DeleteAsync(url);
            return JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// Path 메서드 비동기 호출
        /// T => 제네릭 클래스 외부에서 데이터 변수타입을 입력받아 내부에서 사용
        /// string(JSON) => T 형식으로 변환하여 리턴 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<T> PatchAsync<T>(string url, object input)
        {
            string str = await PatchAsync(url, input);
            return JsonConvert.DeserializeObject<T>(str);
        }


    }
}
