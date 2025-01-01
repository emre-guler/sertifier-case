using SertifierCase.Data.Enums;

namespace SertifierCase.Data.Entity;

public class Attendee : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public AttendeeType Type { get; set; }
}