namespace ClothesShop.Queries.ReturnTypes;

public class CompletedOrder
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal TotalOrderCost { get; set; }

    public int ProductCount { get; set; }

    public DateTime CompleteTime { get; set; }

}
