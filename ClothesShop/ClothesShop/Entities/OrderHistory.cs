namespace ClothesShop.Entities;

public class OrderHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int StatusId { get; set; }

    public DateTime SetTime { get; set; }

    public virtual Order Order { get; set; }

    public virtual Status Status { get; set; }
}
