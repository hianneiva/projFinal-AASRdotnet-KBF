using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace KnowledgeBaseForum.AdminWebApp.Utils
{
    /// <summary>
    /// Class that encapsulates HTTP Request/Response logic.
    /// </summary>
    /// <typeparam name="T">Expected response content type.</typeparam>
    /// <typeparam name="U">Request content type.</typeparam>
    public class HttpHelper<T, U>
    {
        private const string JSON_MIME_TYPE = "application/json";
        private readonly HttpClient client;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="factory">Dependency injection HTTP client factory.</param>
        /// <param name="baseUrl">The target host base URL address.</param>
        public HttpHelper(IHttpClientFactory factory, string baseUrl, string? authHeader = null)
        {
            client = factory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JSON_MIME_TYPE));

            if (!string.IsNullOrEmpty(authHeader))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader);
            }
        }

        /// <summary>
        /// Destructor: also disposes of initialized HTTP client.
        /// </summary>
        ~HttpHelper()
        {
            client.Dispose();
        }

        /// <summary>
        /// Makes a GET request to the target host.
        /// </summary>
        /// <param name="requestUrl">Complementary URL address (sub-directory).</param>
        /// <returns>The parsed content as the expected object.</returns>
        public async Task<T?> Get(string requestUrl)
        {
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw await ContextualizedExceptionGenerator(response);
            }

            return await ParseResponseContext(response);
        }

        /// <summary>
        /// Makes a POST request to the target host.
        /// </summary>
        /// <param name="requestUrl">Complementary URL address (sub-directory).</param>
        /// <param name="requestContent">The content to be sent in the request.</param>
        /// <returns>The parsed content as the expected object.</returns>
        public async Task<T?> Post(string requestUrl, U requestContent)
        {
            HttpContent httpReqContent = PrepareRequestContent(requestContent);
            HttpResponseMessage response = await client.PostAsync(requestUrl, httpReqContent);

            if (!response.IsSuccessStatusCode)
            {
                throw await ContextualizedExceptionGenerator(response);
            }

            return await ParseResponseContext(response);
        }

        /// <summary>
        /// Makes a PUT request to the target host.
        /// </summary>
        /// <param name="requestUrl">Complementary URL address (sub-directory).</param>
        /// <param name="requestContent">The content to be sent in the request.</param>
        /// <returns>The parsed content as the expected object.</returns>
        public async Task<T?> Put(string requestUrl, U requestContent)
        {
            HttpContent httpReqContent = PrepareRequestContent(requestContent);
            HttpResponseMessage response = await client.PutAsync(requestUrl, httpReqContent);

            if (!response.IsSuccessStatusCode)
            {
                throw await ContextualizedExceptionGenerator(response);
            }

            return await ParseResponseContext(response);
        }

        /// <summary>
        /// Makes a DELETE request to the target host.
        /// </summary>
        /// <param name="requestUrl">Complementary URL address (sub-directory).</param>
        /// <returns>The parsed content as the expected object.</returns>
        public async Task<T?> Delete(string requestUrl)
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

        private async Task<T?> ParseResponseContext(HttpResponseMessage response)
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
