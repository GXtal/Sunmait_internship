using Web.Models.ViewModels;

namespace Web.Hubs.ClientInterfaces;

public interface IProductCountClient
{
    Task GetProductCount(ProductCountViewModel productCount);
}
