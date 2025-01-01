using Microsoft.AspNetCore.Mvc;
using SertifierCase.Data.Entity;
using SertifierCase.Infrastructure.Errors;
using SertifierCase.Infrastructure.Models;
using SertifierCase.Services.AttendeeService;
using SertifierCase.Services.Models;

namespace SertifierCase.API.Controllers;

[ApiController]
[Route("api/attendees/")]

public class AttendeeController : ControllerBase
{
    private readonly IAttendeeService _attendeeService;
    public AttendeeController(IAttendeeService attendeeService)
    {
        _attendeeService = attendeeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttendee model)
    {
        Attendee attendee = await _attendeeService.Create(model);
        if (attendee.Id == default(Guid)) throw new SertifierException(CustomErrors.E_100);
        return Ok(new Response(false, "success", attendee, null));
    }

    [HttpPost("{attendeeId:Guid}/EnrollAttendee")]
    public async Task<IActionResult> EnrollAttendeeToCourse([FromRoute] Guid attendeeId, [FromBody] EnrollAttendee model)
    {
        await _attendeeService.EnrollToCourse(attendeeId, model.courseId);
        return Ok(new Response(false, "success", null, null));
    }

    [HttpGet("LeaderBoard")]
    public async Task<IActionResult> GetLeaderBoard([FromQuery] int limit = 30, int offset = 0)
    {
        limit = limit >= 50 ? 50 : limit;
        LeaderBoard leaderBoardData = await _attendeeService.GetLeaderBoard(limit, offset);
        return Ok(new Response(false, "succes", leaderBoardData.LeaderBoardList, new MetaData(offset, limit, leaderBoardData.Count)));
    }
}