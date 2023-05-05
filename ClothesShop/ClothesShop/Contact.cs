namespace ClothesShop;

public class Contact
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
