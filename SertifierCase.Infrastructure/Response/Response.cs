namespace SertifierCase.Infrastructure.Models;

public record Response(bool hasError, string message, object? data, MetaData? metaData){};
public record MetaData(int offset, int limit, int count){};

public record CourseList
{
    public int Count { get; set;}
    public List<CourseListItem> CourseListItem { get; set;}
}
public class CourseListItem
{
    public string Title { get; set; }
    public string DeliveryId { get; set; }
}