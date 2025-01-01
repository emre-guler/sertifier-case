namespace SertifierCase.Services.Models;


public class LeaderBoard 
{
    public int Count { get; set; }
    public List<LeaderBoardListItem> LeaderBoardList { get; set; } 
}
public class LeaderBoardListItem
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public int CountCourseFinished { get; set;}

}