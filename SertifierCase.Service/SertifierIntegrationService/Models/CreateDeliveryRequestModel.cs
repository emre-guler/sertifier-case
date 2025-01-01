using Newtonsoft.Json;

namespace SertifierCase.Services.SertifierIntegrationService.Models;

public class CreateDeliveryRequestModel
{
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("mailSubject")]
    public string MailSubject { get; set; }
    [JsonProperty("mailBody")]
    public string MailBody { get; set; }
    [JsonProperty("designId")]
    public Guid DesignId { get; set; }
    [JsonProperty("badgeId")]
    public Guid BadgeId { get; set; }
    [JsonProperty("detailId")]
    public Guid DetailId { get; set; }
    [JsonProperty("emailTemplateId")]
    public Guid EmailTemplateId { get; set; }
    [JsonProperty("emailFromName")]
    public string EmailFromName { get; set; }
}