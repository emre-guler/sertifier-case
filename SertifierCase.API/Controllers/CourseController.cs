using Microsoft.AspNetCore.Mvc;
using SertifierCase.Services.SertifierIntegrationService;
using SertifierCase.Data.Entity;
using SertifierCase.Infrastructure.Errors;
using SertifierCase.Infrastructure.Models;
using SertifierCase.Services.CourseService;
using SertifierCase.Services.Models;

namespace SertifierCase.API.Controllers;

[ApiController]
[Route("api/courses/")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ISertifierIntegrationService _integrationService;
    public CourseController(ICourseService courseService, ISertifierIntegrationService integrationService)
    {
        _courseService = courseService;
        _integrationService = integrationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourse model)
    {
        Course course = await _courseService.Create(model);
        if (course == default(Course)) throw new SertifierException(CustomErrors.E_100);
        await _integrationService.DetailAndDeliveryIntegration(course);
        return Ok(new Response(false, "success", course, null));
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] int limit = 30, int offset = 0, string query = "")
    {
        limit = limit >= 50 ? 50 : limit;
        CourseList courseData = await _courseService.ListCourses(limit, offset, query);
        return Ok(new Response(false, "success", courseData.CourseListItem, new MetaData(offset, limit, courseData.Count)));
    }
}
