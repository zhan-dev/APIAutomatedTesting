using NUnit.Framework;
using System.Net;

namespace APITestAutomation.Tests.src
{
    public class TypicodeFixture
    {
        private readonly TypicodeClient typicodeClient = new($"https://jsonplaceholder.typicode.com/users");

        [Test]
        public async Task ValidateThatListOfUsersCanBeReceivedAndFieldsAreNotNullOrEmpty()
        {
            var response = await this.typicodeClient.GetTypicodesResponseAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Count, Is.GreaterThan(0));
        }

        //Validate response header for a list of users
        [Test]
        public async Task ValidateResponseHeaderForListOfUsers()
        {
            var response = await this.typicodeClient.GetTypicodesResponseAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.Headers, Is.Not.Null.And.Not.Empty);

            var contentTypeHeader = response.Headers!
                .FirstOrDefault(contentType => contentType.Name.Equals("Content-Type", StringComparison.OrdinalIgnoreCase));

            Assert.That(contentTypeHeader, Is.Not.Null);
            Assert.That(contentTypeHeader!.Value.ToString(), Is.EqualTo("application/json; charset=utf-8"));
        }

        [Test]
        public async Task ValidateResponseContentForListOfUsers()
        {
            var response = await this.typicodeClient.GetTypicodesResponseAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Count, Is.EqualTo(10));

            var ids = response.Data!.Select(data => data.Id).ToList();
            Assert.That(ids.Distinct().Count(), Is.EqualTo(ids.Count));

            Assert.Multiple(() =>
            {
                foreach (var user in response.Data)
                {
                    Assert.That(user.Name, Is.Not.Null.And.Not.Empty);
                    Assert.That(user.Username, Is.Not.Null.And.Not.Empty);
                    Assert.That(user.Company, Is.Not.Null);
                    Assert.That(user.Company.Name, Is.Not.Null.And.Not.Empty);
                }
            });
        }
    }
}
