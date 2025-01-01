using SertifierCase.Data.Entity;
using SertifierCase.Services.Models;

namespace SertifierCase.Services.AttendeeService;

public interface IAttendeeService
{
    Task<Attendee?> GetById(Guid attendeeId);
    Task<Attendee> Create(CreateAttendee model);
    Task EnrollToCourse(Guid attendeeId, Guid courseId);
    Task<LeaderBoard> GetLeaderBoard(int limit = 30, int offset = 0);
}