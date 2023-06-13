namespace Web.Models.ViewModels;

public class OrderProductViewModel
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal ProductPrice { get; set; }

    public int Count { get; set; }
}
