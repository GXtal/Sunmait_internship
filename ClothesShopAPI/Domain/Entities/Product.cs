namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Image> Images { get; set; }

    public virtual ICollection<OrderProduct> OrdersProducts { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}
