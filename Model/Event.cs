using System.Text.Json.Serialization;

namespace Godwit.HandleEmailConfirmedEvent.Model {
    public class Event {
        [JsonPropertyName("session_variables")]
        public SessionVariables Session { get; set; }

        [JsonPropertyName("op")] public string Operation { get; set; }

        [JsonPropertyName("data")] public Data Data { get; set; }
    }
}