namespace ClothesShop.Entities;

public class Status
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<OrderHistory> OrderHistories { get; set; }
}
