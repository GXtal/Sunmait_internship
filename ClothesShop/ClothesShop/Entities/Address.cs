namespace ClothesShop.Entities;

public class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FullAddress { get; set; }

    public virtual User User { get; set; }
}
