namespace ClothesShop;

public class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();
}
