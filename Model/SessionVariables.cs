using System.Text.Json.Serialization;

namespace Godwit.HandleEmailConfirmedEvent.Model {
    public class SessionVariables {
        [JsonPropertyName("x-hasura-role")] public string HasuraRole { get; set; }
    }
}