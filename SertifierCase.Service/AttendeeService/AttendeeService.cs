using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SertifierCase.Data.Context;
using SertifierCase.Data.Entity;
using SertifierCase.Data.Enums;
using SertifierCase.Infrastructure.Errors;
using SertifierCase.Services.CourseService;
using SertifierCase.Services.Models;

namespace SertifierCase.Services.AttendeeService;

public class AttendeeService : IAttendeeService
{
    private readonly SertifierCaseContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICourseService _courseService;
    public AttendeeService(SertifierCaseContext dbContext, IMapper mapper, ICourseService courseService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _courseService = courseService;
    }

    public async Task<Attendee?> GetById(Guid attendeeId)
    {
        return await _dbContext.Attendees.FirstOrDefaultAsync(x => x.Id == attendeeId);
    }

    public async Task<Attendee> Create(CreateAttendee model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Name)) throw new SertifierException(CustomErrors.E_101);

        var isEmailExist = await _dbContext.Attendees.AnyAsync(x => x.Email.Equals(model.Email));
        if (isEmailExist) throw new SertifierException(CustomErrors.E_102);

        Attendee newAttendee = _mapper.Map<Attendee>(model);

        newAttendee.Type = AttendeeType.Employed;
        newAttendee.CreateDate = DateTime.UtcNow;
        newAttendee.UpdateDate = DateTime.UtcNow;

        _dbContext.Attendees.Add(newAttendee);
        await _dbContext.SaveChangesAsync();

        return newAttendee;
    }

    public async Task EnrollToCourse(Guid attendeeId, Guid courseId)
    {
        Attendee? attendee = await GetById(attendeeId);
        if (attendee == null)
            throw new SertifierException(CustomErrors.E_103);

        Course? course = await _courseService.GetById(courseId);
        if (course == null)
            throw new SertifierException(CustomErrors.E_104);

        bool isExist = await _dbContext.CourseAttendees.AnyAsync(x => x.AttendeeId == attendeeId && x.CourseId == courseId);
        if (isExist)
            throw new SertifierException(CustomErrors.E_105);

        await _dbContext.CourseAttendees.AddAsync(new CourseAttendee
        {
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
            CourseId = courseId,
            AttendeeId = attendeeId
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<LeaderBoard> GetLeaderBoard(int limit = 30, int offset = 0)
    {
        /*
            SELECT 
                att.Id AS AttendeeId, 
                att.Name AS AttendeeName, 
                COUNT(cre.CourseId) AS CourseFinished 
            FROM 
                Credentials cre 
            LEFT OUTER JOIN Attendees att ON att.Id = cre.AttendeeId 
            GROUP BY att.Id
        */
        var query = (from cre in _dbContext.Credentials
                    join att in _dbContext.Attendees on cre.AttendeeId equals att.Id into attGroup
                    from att in attGroup.DefaultIfEmpty()
                    group att by att.Id into g
                    orderby g.Count() descending
                    select new LeaderBoardListItem()
                    {
                        Id = g.Key.ToString(),
                        Email = g.FirstOrDefault().Email,
                        CountCourseFinished = g.Count()
                    });
        return new LeaderBoard()
        {
            Count = await query.CountAsync(),
            LeaderBoardList = await query.Skip(offset).Take(limit).ToListAsync()
        };
    }
}