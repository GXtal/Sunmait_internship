namespace ClothesShop.Entities;

public class Image
{
    public int Id { get; set; }

    public byte[] Content { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; }
}
