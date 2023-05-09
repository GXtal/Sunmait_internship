namespace ClothesShop.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalCost { get; set; }

    public virtual ICollection<OrderHistory> OrderHistories { get; set; }

    public virtual ICollection<OrderProduct> OrdersProducts { get; set; }

    public virtual User User { get; set; }
}
