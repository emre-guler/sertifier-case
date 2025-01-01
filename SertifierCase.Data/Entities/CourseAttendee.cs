using System.ComponentModel.DataAnnotations.Schema;

namespace SertifierCase.Data.Entity;

public class CourseAttendee : BaseEntity
{    
    public Guid CourseId { get; set; }
    [ForeignKey("CourseId")] public virtual Course? Course { get; set; }

    public Guid AttendeeId { get; set; }
    [ForeignKey("AttendeeId")] public virtual Attendee? Attendee { get; set; }
}