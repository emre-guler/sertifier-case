using SertifierCase.Data.Entity;

namespace SertifierCase.Services.SertifierIntegrationService;

public interface ISertifierIntegrationService
{
    Task<Guid> CreateDelivery(Guid detailId);
    Task<Guid> CreateDetail();
    Task RecipientIntegration();
    Task DetailAndDeliveryIntegration(Course course);
}