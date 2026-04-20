using APITestAutomation.Tests.src.Models;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APITestAutomation.Tests.src
{
    public class UnicornClient
    {
        private readonly IRestClient _client;

        public UnicornClient(string endpoint)
        {
            var serializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            this._client = new RestClient(
                options: new() 
                { 
                    BaseUrl = new Uri(endpoint) 
                },
                configureSerialization: s => s.UseSystemTextJson(serializerOptions));
        }

        public async Task<List<Unicorn>> GetUnicornsAsync()
        {
            var request = new RestRequest($"/unicorns", Method.Get);
            var response = await this._client.GetAsync<List<Unicorn>>(request);
            return response;
        }

        public async Task<Unicorn> CreateUnicornAsync(Unicorn unicorn)
        {
            var request = new RestRequest($"/unicorns", Method.Post);
            request.AddJsonBody(unicorn);

            var response = await this._client.PostAsync<Unicorn>(request);
            return response;
        }

        public async Task<RestResponse> UpdateUnicornAsync(string id, Unicorn unicorn)
        {
            var request = new RestRequest($"/unicorns/{id}", Method.Put);
            request.AddJsonBody(unicorn);

            var response = await this._client.PutAsync(request);
            return response;
        }

        public async Task<RestResponse> DeleteUnicornAsync(string id)
        {
            var request = new RestRequest($"/unicorns/{id}", Method.Delete);
            var response = await this._client.DeleteAsync(request);
            return response;
        }
    }
}
