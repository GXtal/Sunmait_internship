namespace ClothesShop.Queries.ReturnTypes;

public class CompletedOrder
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal TotalOrderCost { get; set; }

    public int ProductCount { get; set; }

    public DateTime CompleteTime { get; set; }

    public override string ToString()
    {
        return $"Id:{OrderId}, User:{UserId}, Cost:{TotalOrderCost}, count:{ProductCount}, time:{CompleteTime}";
    }

}
