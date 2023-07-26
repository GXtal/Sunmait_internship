namespace Domain.Entities;

public class ReservedProduct
{
    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public DateTime ExpirationTime { get; set; }

    public User User { get; set; }

    public Product Product { get; set; }
}

