namespace ClothesShop;

public partial class Image
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
