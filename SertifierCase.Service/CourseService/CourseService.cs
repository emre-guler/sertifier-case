using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SertifierCase.Data.Context;
using SertifierCase.Data.Entity;
using SertifierCase.Infrastructure.Errors;
using SertifierCase.Infrastructure.Models;
using SertifierCase.Services.Models;

namespace SertifierCase.Services.CourseService;

public class CourseService : ICourseService
{
    private readonly SertifierCaseContext _dbContext;
    private readonly IMapper _mapper;
    public CourseService(SertifierCaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Course?> GetById(Guid courseId)
    {
        return await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
    }

    public async Task<Course> Create(CreateCourse model)
    {
        if (String.IsNullOrWhiteSpace(model.Title)) throw new SertifierException(CustomErrors.E_101);

        Course newCourse = _mapper.Map<Course>(model);
        newCourse.CreateDate = DateTime.UtcNow;
        newCourse.UpdateDate = DateTime.UtcNow;
        _dbContext.Courses.Add(newCourse);
        await _dbContext.SaveChangesAsync();

        return newCourse;
    }

    public async Task UpdateDeliveryId(Course model, Guid deliveryId)
    {
        model.DeliveryId = deliveryId;
        model.UpdateDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CourseList> ListCourses(int limit = 30, int offset = 0, string query = "")
    {
        var qry = _dbContext
            .Courses
            .Where(x => x.Title.Contains(query));
        return new CourseList()
        {
            Count = await qry.CountAsync(),
            CourseListItem = await qry.OrderByDescending(x => x.CreateDate)
                            .Skip(offset)
                            .Take(limit)
                            .ProjectToType<CourseListItem>(_mapper.Config)
                            .AsNoTracking()
                            .ToListAsync()
        };
    }

    public async Task<List<CourseAttendee>> GetCompletedCourseAttendees()
    {
        // get between 5 minutes and 25 minutes 
        return await _dbContext.CourseAttendees
            .Include(x => x.Course)
            .Where(x => x.CreateDate <= DateTime.UtcNow.AddMinutes(-5) && x.CreateDate >= DateTime.UtcNow.AddMinutes(-25))
            .ToListAsync();
    }

    public async Task<Course?> GetCourseByDeliveryId(Guid deliveryId)
    {
        return await _dbContext.Courses
            .Where(x => x.DeliveryId == deliveryId)
            .FirstOrDefaultAsync();
    }
}