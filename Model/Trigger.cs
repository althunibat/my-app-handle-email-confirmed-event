using System.Text.Json.Serialization;

namespace Godwit.HandleEmailConfirmedEvent.Model {
    public class Trigger {
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}