namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public int AvailableQuantity { get; set; }

    public int ReservedQuantity { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public Brand Brand { get; set; }

    public Category Category { get; set; }

    public ICollection<Image> Images { get; set; }

    public ICollection<OrderProduct> OrdersProducts { get; set; }

    public ICollection<ReservedProduct> ReservedProducts { get; set; }

    public ICollection<Review> Reviews { get; set; }
}
