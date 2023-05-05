namespace ClothesShop;

public class Image
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
