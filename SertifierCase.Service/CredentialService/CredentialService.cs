using Microsoft.EntityFrameworkCore;
using SertifierCase.Data.Context;
using SertifierCase.Data.Entity;
using SertifierCase.Services.AttendeeService;
using SertifierCase.Services.CourseService;

namespace SertifierCase.Services.CredentialService;
public class CredentialService : ICredentialService
{
    private readonly SertifierCaseContext _dbContext;
    private readonly ICourseService _courseService;
    private readonly IAttendeeService _attendeeService;
    public CredentialService(SertifierCaseContext dbContext, ICourseService courseService, IAttendeeService attendeeService)
    {
        _dbContext = dbContext;
        _courseService = courseService;
        _attendeeService = attendeeService;
    }

    public async Task<Credential?> GetCredential(Guid attendeeId, Guid courseId)
    {
        return await _dbContext.Credentials
        .FirstOrDefaultAsync(x => x.AttendeeId == attendeeId && x.CourseId == courseId);
    }
    
    public async Task Create(Credential credential)
    {
        if (await _courseService.GetById(credential.CourseId) is null)  return;
        if (await _attendeeService.GetById(credential.AttendeeId) is null) return;
        
        await _dbContext.Credentials.AddAsync(credential);
        await _dbContext.SaveChangesAsync();
    }
}