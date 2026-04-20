using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIAutomatedTesting.src
{
    public class UnicornFixture
    {
        private const string identifier = "ecd8bd81e38b4104b0634fbfa3822a65";
        private IRestClient _restClient;

        [OneTimeSetUp]
        public void Before()
        {
            var serializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            this._restClient = new RestClient(
                options: new()
                {
                    BaseUrl = new Uri($"https://crudcrud.com/api/{identifier}")
                },
                configureSerialization: s => s.UseSystemTextJson(serializerOptions));
        }

        [Test]
        public async Task VerifyThatGetAllUnicornsResourceReturnsOkStatusCode()
        {
            var getAllUnicornsRequest = new RestRequest($"/unicorns", Method.Get);
            var getAllUnicornsResponse = await this._restClient.ExecuteAsync<Unicorn[]>(getAllUnicornsRequest);

            Assert.That(getAllUnicornsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task VerifyThatUnicornCanBeCreated()
        {
            var unicorn = new Unicorn
            {
                Name = $"Name_{Random.Shared.Next(100, 1000)}",
                Age = Random.Shared.Next(1, 10),
                Colour = "red"
            };

            var createUnicornRequest = new RestRequest("/unicorns", Method.Post);
            createUnicornRequest.AddBody(unicorn);

            var createUnicornResponse = await this._restClient.ExecuteAsync<Unicorn>(createUnicornRequest);

            Assert.That(createUnicornResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(createUnicornResponse.Data.Id, Is.Not.Null.Or.Empty);
            Assert.That(createUnicornResponse.Data.Name, Is.EqualTo(unicorn.Name));
        }

        [Test]
        public async Task VerifyThatUnicornCanBeRetrievedByIdentifier()
        {
            var unicorn = new Unicorn
            {
                Name = $"Name_{Random.Shared.Next(100, 1000)}",
                Age = Random.Shared.Next(1, 10),
                Colour = "red"
            };

            var createUnicornRequest = new RestRequest("/unicorns", Method.Post);
            createUnicornRequest.AddBody(unicorn);

            var createUnicornResponse = await _restClient.ExecuteAsync<Unicorn>(createUnicornRequest);
            var createdUnicorn = createUnicornResponse.Data;

            var getUnicornByIdentifierRequest = new RestRequest($"/unicorns/{createdUnicorn.Id}", Method.Get);
            var getUnicornByIdentifierResponse = await _restClient.ExecuteAsync<Unicorn>(getUnicornByIdentifierRequest);

            Assert.That(getUnicornByIdentifierResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getUnicornByIdentifierResponse.Data.Id, Is.EqualTo(createdUnicorn.Id));
        }

        [Test]
        public async Task VerifyThatUnicornCanBeEdited()
        {
            var unicorn = new Unicorn
            {
                Name = $"Name_{Random.Shared.Next(100, 1000)}",
                Age = Random.Shared.Next(1, 10),
                Colour = "red"
            };

            var createUnicornRequest = new RestRequest("/unicorns", Method.Post);
            createUnicornRequest.AddBody(unicorn);

            var createUnicornResponse = await _restClient.ExecuteAsync<Unicorn>(createUnicornRequest);
            var createdUnicorn = createUnicornResponse.Data;

            unicorn.Name += "_edited";
            unicorn.Age += 5;

            var editUnicornRequest = new RestRequest($"/unicorns/{createdUnicorn.Id}", Method.Put);
            editUnicornRequest.AddBody(unicorn);

            var editUnicornResponse = await _restClient.ExecuteAsync(editUnicornRequest);

            Assert.That(editUnicornResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getUnicornRequest = new RestRequest($"/unicorns/{createdUnicorn.Id}", Method.Get);
            var getUnicornResponse = await _restClient.ExecuteAsync<Unicorn>(getUnicornRequest);

            Assert.That(getUnicornResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getUnicornResponse.Data.Name, Is.EqualTo(unicorn.Name));
            Assert.That(getUnicornResponse.Data.Age, Is.EqualTo(unicorn.Age));
        }

        [Test]
        public async Task VerifyThatUnicornCanBeDeleted()
        {
            var unicorn = new Unicorn
            {
                Name = $"Name_{Random.Shared.Next(100, 1000)}",
                Age = Random.Shared.Next(1, 10),
                Colour = "red"
            };

            var createUnicornRequest = new RestRequest("/unicorns", Method.Post);
            createUnicornRequest.AddBody(unicorn);

            var createUnicornResponse = await _restClient.ExecuteAsync<Unicorn>(createUnicornRequest);
            var createdUnicorn = createUnicornResponse.Data;

            var deleteUnicornRequest = new RestRequest($"/unicorns/{createdUnicorn.Id}", Method.Delete);
            var deleteUnicornResponse = await _restClient.ExecuteAsync(deleteUnicornRequest);

            Assert.That(deleteUnicornResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getAllUnicornsRequest = new RestRequest($"/unicorns", Method.Get);
            var getAllUnicornsResponse = await _restClient.ExecuteAsync<Unicorn[]>(getAllUnicornsRequest);

            Assert.That(createdUnicorn.Id, Is.Not.AnyOf(getAllUnicornsResponse.Data.Select(u => u.Id)));
        }
    }
}
