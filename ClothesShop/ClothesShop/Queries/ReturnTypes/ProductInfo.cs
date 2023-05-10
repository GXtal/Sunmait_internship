namespace ClothesShop.Queries.ReturnTypes;

public class ProductInfo
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal ProductPrice { get; set; }

    public string ProductDescription { get; set; }

    public int ProductQuantity { get; set; }

    public string CategoryName { get; set; }

    public string BrandName { get; set; }

    public override string ToString()
    {
        return $"Id:{ProductId}, Name:{ProductName}, Price:{ProductPrice}, Description:{ProductDescription}, Quantity:{ProductQuantity}, Category:{CategoryName}, Brand:{BrandName}";
    }
}
