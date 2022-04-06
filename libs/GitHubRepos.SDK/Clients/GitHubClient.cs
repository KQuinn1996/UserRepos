using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace UserRepos.SDK.Clients
{
    public class GitHubClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "http://api.github.com/users/";

        public GitHubClient(HttpClient httpClient)
        {
            _httpClient = InitializeClient(httpClient);
        }

        private HttpClient InitializeClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(_baseAddress);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Agent"); // needed to allow access, 403 Forbidden otherwise
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public async Task<IResponse> GetResponseAsync<IResponse>(string url)
        {
            var stringResponse = await ReadStringResponseFromUrlAsync(url);
            return JsonConvert.DeserializeObject<IResponse>(stringResponse);
        }

        public async Task<List<IResponse>> GetResponseListAsync<IResponse>(string url)
        {
            var stringResponse = await ReadStringResponseFromUrlAsync(url);
            return JsonConvert.DeserializeObject<List<IResponse>>(stringResponse);
        }

        private async Task<string> ReadStringResponseFromUrlAsync(string url)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
