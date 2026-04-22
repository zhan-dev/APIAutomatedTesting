using APITestAutomation.Tests.src.Models;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APITestAutomation.Tests.src
{
    public class TypicodeClient
    {
        private readonly IRestClient _client;

        public TypicodeClient(string endpoint)
        {
            var serializerOptions = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            this._client = new RestClient(options: new() { BaseUrl = new Uri(endpoint) }, 
                configureSerialization: s => s.UseSystemTextJson(serializerOptions));
        }

        public async Task<List<Typicode>> GetTypicodesAsync()
        {
            var request = new RestRequest($"/users", Method.Get);
            return await this._client.GetAsync<List<Typicode>>(request) ?? new List<Typicode>();
        }

        public async Task<RestResponse<List<Typicode>>> GetTypicodesResponseAsync()
        {
            var request = new RestRequest($"/users", Method.Get);
            return await this._client.ExecuteAsync<List<Typicode>>(request);
        }

        public async Task<Typicode> CreateTypicodeAsync(Typicode user)
        {
            var request = new RestRequest($"/users", Method.Post).AddJsonBody(user);
            return await this._client.PostAsync<Typicode>(request) ?? new Typicode();
        }

        public async Task<RestResponse> UpdateTypicodeAsync(string id, Typicode user)
        {
            var request = new RestRequest($"/users/{id}", Method.Put).AddJsonBody(user);
            return await this._client.PutAsync(request);
        }

        public async Task<RestResponse> DeleteTypicodeAsync(string id)
        {
            var request = new RestRequest($"/users/{id}", Method.Delete);
            return await this._client.DeleteAsync(request);
        }
    }
}
