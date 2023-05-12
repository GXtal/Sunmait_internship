namespace ClothesShop.Queries.ReturnTypes;

public class ProductReview
{
    public int ProductId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public string CommentatorName { get; set; }

    public string CommentatorEmail { get; set;}

    public override string ToString()
    {
        return $"Id:{ProductId}, commentator:{CommentatorName}, comment:{Comment}, rate:{Rating}, email:{CommentatorEmail}";
    }
}
