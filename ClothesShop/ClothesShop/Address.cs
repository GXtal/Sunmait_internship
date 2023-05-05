namespace ClothesShop;

public partial class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FullAddress { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
