using System.Text.Json.Serialization;

namespace Godwit.HandleEmailConfirmedEvent.Model {
    public class User {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("email_confirmed")] public bool IsConfirmed { get; set; }

        [JsonPropertyName("email")] public string Email { get; set; }

        [JsonPropertyName("user_name")] public string UserName { get; set; }

        [JsonPropertyName("first_name")] public string FirstName { get; set; }

        [JsonPropertyName("last_name")] public string LastName { get; set; }

        [JsonIgnore] public string Name => $"{FirstName} {LastName}";
    }
}