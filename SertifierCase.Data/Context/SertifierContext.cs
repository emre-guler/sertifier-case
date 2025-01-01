using Microsoft.EntityFrameworkCore;
using SertifierCase.Data.Entity;

namespace SertifierCase.Data.Context;

public class SertifierCaseContext : DbContext
{
    public SertifierCaseContext(DbContextOptions<SertifierCaseContext> options) : base(options) 
    {
        
    }

    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Attendee> Attendees { get; set; }
    public virtual DbSet<Credential> Credentials { get; set; }
    public virtual DbSet<CourseAttendee> CourseAttendees { get; set; }
}