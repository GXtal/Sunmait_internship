namespace Domain.Entities;

public class OrderProduct
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public Order Order { get; set; }

    public Product Product { get; set; }
}
