using Web.Models.ViewModels;

namespace Web.Hubs.ClientInterfaces;

public interface IProductViewersCountClient
{
    Task GetViewersCount(ProductViewersCountViewModel viewersCount);
}
