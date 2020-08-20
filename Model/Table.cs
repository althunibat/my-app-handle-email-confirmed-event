using System.Text.Json.Serialization;

namespace Godwit.HandleEmailConfirmedEvent.Model {
    public class Table {
        [JsonPropertyName("schema")] public string Schema { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }
    }
}