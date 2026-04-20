using System.Text.Json.Serialization;

namespace APIAutomatedTesting.src
{
    public class Unicorn
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("colour")]
        public string Colour { get; set; }
    }
}
