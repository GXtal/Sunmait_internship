namespace ClothesShop.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalCost { get; set; }

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual ICollection<OrderProduct> OrdersProducts { get; set; } = new List<OrderProduct>();

    public virtual User User { get; set; }
}
