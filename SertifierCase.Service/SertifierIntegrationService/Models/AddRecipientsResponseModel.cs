using Newtonsoft.Json;

namespace SertifierCase.Services.SertifierIntegrationService.Models;

public class AddRecipientsResponseModel : BaseResponseModel
{
    public dynamic Data { get; set; }
}

public class AddRecipientsData
{
    public Dictionary<Guid, List<AddRecipientsDataDetail>> Data { get; set;}
}

public class AddRecipientsDataDetail
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string IssueDate { get; set; }
    public bool QuickPublish { get; set; }
    public List<AddRecipientAttribute> Attributes { get; set; }
    public Guid ExternalId { get; set; }
    public string CertificateNo { get; set; }
    public int Status { get; set; }
    public string CertificateImageLink { get; set; }
    public string BadgeImageLink { get; set; }
    public string VerificationLink { get; set; }
}