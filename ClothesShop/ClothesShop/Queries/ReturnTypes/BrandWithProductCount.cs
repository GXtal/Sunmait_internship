namespace ClothesShop.Queries.ReturnTypes;

public class BrandWithProductCount
{
    public int BrandId { get; set; }

    public string BrandName { get; set; }

    public long ProductsCount { get; set; }

    public override string ToString()
    {
        return $"Id:{BrandId}, Name: {BrandName}, Count:{ProductsCount}";
    }
}
