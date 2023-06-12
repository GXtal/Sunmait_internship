namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalCost { get; set; }

    public int StatusId { get; set; }

    public Status Status { get; set; }

    public ICollection<OrderHistory> OrderHistories { get; set; }

    public ICollection<OrderProduct> OrdersProducts { get; set; }

    public User User { get; set; }
}
