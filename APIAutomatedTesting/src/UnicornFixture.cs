using APITestAutomation.Tests.src.Models;
using NUnit.Framework;

namespace APITestAutomation.Tests.src
{
    public class UnicornFixture
    {
        private readonly UnicornClient unicornClient = new($"https://crudcrud.com/api/{Constants.Identifier}");

        [Test]
        public async Task VerifyThatUnicornsCanBeRetrieved()
        {
            var unicorns = await this.unicornClient.GetUnicornsAsync();

            Assert.That(unicorns.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task VerifyThatUnicornCanBeCreated()
        {
            var unicornToCreate = new Unicorn
            {
                Name = $"Name_{Random.Shared.Next(100, 1000)}",
                Age = Random.Shared.Next(1, 10),
                Colour = "red"
            };

            var response = await this.unicornClient.CreateUnicornAsync(unicornToCreate);

            Assert.That(response.Id, Is.Not.Null.Or.Empty);
            Assert.That(response.Name, Is.EqualTo(unicornToCreate.Name));
        }

        [Test]
        public async Task VerifyThatUnicornCanBeUpdated()
        {
            var unicornToEdit = new Unicorn
            {
                Name = $"Name_{Random.Shared.Next(100, 1000)}",
                Age = Random.Shared.Next(1, 10),
                Colour = "red"
            };

            var createdUnicorn = await this.unicornClient.CreateUnicornAsync(unicornToEdit);

            var expectedUnicorn = new Unicorn
            {
                Name = $"{unicornToEdit}_edited",
                Age = 15,
            };

            await this.unicornClient.UpdateUnicornAsync(createdUnicorn.Id, expectedUnicorn);

            var unicorns = await this.unicornClient.GetUnicornsAsync();
            var actualUnicorn = unicorns.First(u => u.Id == createdUnicorn.Id);

            Assert.That(actualUnicorn.Name, Is.EqualTo(expectedUnicorn.Name));
            Assert.That(actualUnicorn.Age, Is.EqualTo(expectedUnicorn.Age));
        }

        [Test]
        public async Task VerifyThatUnicornCanBeDeleted()
        {
            var unicornToDelete = new Unicorn
            {
                Name = $"Name_{Random.Shared.Next(100, 1000)}",
                Age = Random.Shared.Next(1, 10),
                Colour = "red"
            };

            var createdUnicorn = await this.unicornClient.CreateUnicornAsync(unicornToDelete);

            await this.unicornClient.DeleteUnicornAsync(createdUnicorn.Id);

            var unicorns = await this.unicornClient.GetUnicornsAsync();
            var actualUnicorn = unicorns.Find(u => u.Id == createdUnicorn.Id);

            Assert.That(actualUnicorn, Is.Null);
        }
    }
}
