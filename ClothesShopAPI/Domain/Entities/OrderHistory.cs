namespace Domain.Entities;

public class OrderHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int StatusId { get; set; }

    public DateTime SetTime { get; set; }

    public Order Order { get; set; }

    public Status Status { get; set; }
}
