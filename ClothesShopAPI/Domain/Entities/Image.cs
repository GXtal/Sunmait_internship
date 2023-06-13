namespace Domain.Entities;

public class Image
{
    public int Id { get; set; }

    public byte[] Content { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }
}
