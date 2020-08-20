using System;
using System.Text.Json.Serialization;

namespace Godwit.HandleEmailConfirmedEvent.Model {
    public class HasuraEvent {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("table")] public Table Table { get; set; }

        [JsonPropertyName("trigger")] public Trigger Trigger { get; set; }

        [JsonPropertyName("event")] public Event Event { get; set; }

        [JsonPropertyName("delivery_info")] public DeliveryInfo DeliveryInfo { get; set; }

        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    }
}