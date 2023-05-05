namespace ClothesShop;

public partial class Review
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public int? UserId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User? User { get; set; }
}
