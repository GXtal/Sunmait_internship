namespace Web.Models.ViewModels;

public class OrderViewModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalCost { get; set; }

    public int StatusId { get; set; }
}
