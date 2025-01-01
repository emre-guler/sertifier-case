using SertifierCase.Data.Entity;
using SertifierCase.Infrastructure.Models;
using SertifierCase.Services.Models;

namespace SertifierCase.Services.CourseService;
public interface ICourseService
{
    Task<Course?> GetById(Guid courseId);
    Task<Course> Create(CreateCourse model);
    Task UpdateDeliveryId(Course model, Guid deliveryId);
    Task<CourseList> ListCourses(int limit = 30, int offset = 0, string query = "");
    Task<List<CourseAttendee>> GetCompletedCourseAttendees();
    Task<Course?> GetCourseByDeliveryId(Guid deliveryId);
}