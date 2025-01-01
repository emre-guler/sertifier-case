using Newtonsoft.Json;

namespace SertifierCase.Services.SertifierIntegrationService.Models;

public class RecipientRequestModel
{
    [JsonProperty("recipients")]
    public List<Recipient> Recipients { get; set; }
}

public class Recipient
{
    [JsonProperty("deliveryId")]
    public Guid DeliveryId { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("issueDate")]
    public DateTime IssueDate { get; set; }
    [JsonProperty("quickPublish")]
    public bool QuickPublish { get; set; }
    [JsonProperty("attributes")]
    public List<AddRecipientAttribute> Attributes { get; set; }
    [JsonProperty("externalId")]
    public string ExternalId { get; set; }
}

public class AddRecipientAttribute
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; }
}
