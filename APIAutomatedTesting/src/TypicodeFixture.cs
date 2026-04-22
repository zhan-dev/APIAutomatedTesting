using APITestAutomation.Tests.src.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace APITestAutomation.Tests.src
{
    public class TypicodeFixture
    {
        private readonly TypicodeClient typicodeClient = new($"https://jsonplaceholder.typicode.com");

        [Test]
        public async Task ValidateThatListOfUsersCanBeReceivedAndFieldsAreNotNullOrEmpty()
        {
            var response = await this.typicodeClient.GetTypicodesResponseAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task ValidateResponseHeaderForListOfUsers()
        {
            var response = await this.typicodeClient.GetTypicodesResponseAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.ContentType, Is.Not.Null);
            Assert.That(response.ContentType!.ToString(), Is.EqualTo("application/json"));

            Console.WriteLine("Headers: " + string.Join(Environment.NewLine, response.Headers));
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

        [Test]
        public async Task ValidateThatUserCanBeCreated()
        {
            var userToCreate = new Typicode()
            {
                Name = $"Name {Random.Shared.Next(1, 1000)}",
                Username = $"UserName {Random.Shared.Next(1, 1000)}"
            };

            var response = await this.typicodeClient.CreateTypicodeResponseAsync(userToCreate);
            var createdUser = response.Data;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser?.Id, Is.Not.Null.Or.Not.Empty);
            Assert.That(createdUser!.Id, Is.GreaterThan(0));

            Assert.Multiple(() =>
            {
                Assert.That(createdUser.Name, Is.EqualTo(userToCreate.Name));
                Assert.That(createdUser.Username, Is.EqualTo(userToCreate.Username));
            });
        }

        [Test]
        public async Task ValidateThatUserIsNotifiedIfResourceDoesNotExist()
        {
            var response = await this.typicodeClient.ExecuteNotFoundAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(response.Content, Is.EqualTo("{}"));
        }
    }
}
