namespace SertifierCase.Data.Entity;

public class Course : BaseEntity
{
    public required string Title { get; set; }
    public Guid DeliveryId { get; set; }
}