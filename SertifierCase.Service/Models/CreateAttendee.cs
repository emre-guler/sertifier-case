using SertifierCase.Data.Enums;

namespace SertifierCase.Services.Models;

public record CreateAttendee(string Name, string Email, AttendeeType AttendeeType);