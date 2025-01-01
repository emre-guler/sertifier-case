using System.ComponentModel.DataAnnotations.Schema;

namespace SertifierCase.Data.Entity;

public class Credential : BaseEntity
{
    public Guid AttendeeId { get; set; }
    [ForeignKey("AttendeeId")] public virtual Attendee? Attendee { get; set; }

    public Guid CourseId { get; set; }
    [ForeignKey("CourseId")] public virtual Course? Course { get; set; }

    public required string CredentialNo { get; set; }
}