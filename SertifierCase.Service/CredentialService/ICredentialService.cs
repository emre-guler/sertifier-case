using SertifierCase.Data.Entity;

namespace SertifierCase.Services.CredentialService;

public interface ICredentialService
{
    Task<Credential?> GetCredential(Guid attendeeId, Guid courseId);
    Task Create(Credential credential);
}