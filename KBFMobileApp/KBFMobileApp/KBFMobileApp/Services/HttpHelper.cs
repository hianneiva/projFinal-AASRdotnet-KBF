using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KBFMobileApp.Services
{
    public class HttpHelper<T, U>
    {
        private const string JSON_MIME_TYPE = "application/json";
        private readonly HttpClient client;

        public HttpHelper(string baseUrl, string authHeader = null)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JSON_MIME_TYPE));

            if (!string.IsNullOrEmpty(authHeader))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader);
            }
        }

        ~HttpHelper()
        {
            client.Dispose();
        }

        public async Task<T> Get(string requestUrl)
        {
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw await ContextualizedExceptionGenerator(response);
            }

            return await ParseResponseContext(response);
        }

        public async Task<T> Post(string requestUrl, U requestContent)
        {
            HttpContent httpReqContent = PrepareRequestContent(requestContent);
            HttpResponseMessage response = await client.PostAsync(requestUrl, httpReqContent);

            if (!response.IsSuccessStatusCode)
            {
                throw await ContextualizedExceptionGenerator(response);
            }

            return await ParseResponseContext(response);
        }

        public async Task<T> Put(string requestUrl, U requestContent)
        {
            HttpContent httpReqContent = PrepareRequestContent(requestContent);
            HttpResponseMessage response = await client.PutAsync(requestUrl, httpReqContent);

            if (!response.IsSuccessStatusCode)
            {
                throw await ContextualizedExceptionGenerator(response);
            }

            return await ParseResponseContext(response);
        }

        public async Task<T> Delete(string requestUrl)
        {
            HttpResponseMessage response = await client.DeleteAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw await ContextualizedExceptionGenerator(response);
            }

            return await ParseResponseContext(response);
        }

        private HttpContent PrepareRequestContent(U content)
        {
            string jsonReqContent = JsonConvert.SerializeObject(content);
            return new StringContent(jsonReqContent, Encoding.UTF8, JSON_MIME_TYPE);
        }

        private async Task<T> ParseResponseContext(HttpResponseMessage response)
        {
            string jsonContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        private async Task<Exception> ContextualizedExceptionGenerator(HttpResponseMessage response)
        {
            HttpStatusCode code = response.StatusCode;
            string responseContent = await response.Content.ReadAsStringAsync();
            Exception reqException = new HttpRequestException($"Falha - Código: {code} - Detalhe: {responseContent}");
            reqException.Data.Add("STATUS", code);
            reqException.Data.Add("CONTENT", responseContent);

            return reqException;
        }
    }
}
