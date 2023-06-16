using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject;
public class BaseUnitTest
{
    protected readonly ShopDbContext _shopDbContext;

    protected Mock<IOrderRepository> _orderRepository;
    protected Mock<IUserRepository> _userRepository;
    protected Mock<IProductRepository> _productRepository;
    protected Mock<IOrderHistoryRepository> _orderHistoryRepository;
    protected Mock<IOrderProductRepository> _orderProductRepository;
    protected Mock<IStatusRepository> _statusRepository;
    protected Mock<IBrandRepository> _brandRepository;
    protected Mock<ICategoryRepository> _categoryRepository;

    public BaseUnitTest()
    {
        var builder = new DbContextOptionsBuilder<ShopDbContext>();
        builder.UseInMemoryDatabase(databaseName: "ShopDb");

        var dbContextOptions = builder.Options;
        _shopDbContext = new ShopDbContext(dbContextOptions);

        _shopDbContext.Database.EnsureDeleted();
        _shopDbContext.Database.EnsureCreated();

        _orderRepository = new Mock<IOrderRepository>(MockBehavior.Strict);
        _orderHistoryRepository = new Mock<IOrderHistoryRepository>(MockBehavior.Strict);
        _orderProductRepository = new Mock<IOrderProductRepository>(MockBehavior.Strict);
        _productRepository = new Mock<IProductRepository>(MockBehavior.Strict);
        _statusRepository = new Mock<IStatusRepository>(MockBehavior.Strict);
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _brandRepository = new Mock<IBrandRepository>(MockBehavior.Strict);
        _categoryRepository = new Mock<ICategoryRepository>(MockBehavior.Strict);
    }

    public void SetupOrderRepository()
    {
        _orderRepository.Setup(x => x.GetOrderById(It.IsAny<int>())).Returns(async (int id) =>
        {
            var order = await _shopDbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
            return order;
        });

        _orderRepository.Setup(x => x.AddOrder(It.IsAny<Order>())).Returns(async (Order order) =>
        {
            _shopDbContext.Add(order);
            await _shopDbContext.SaveChangesAsync();
            return order;
        });

        _orderRepository.Setup(x => x.UpdateOrder(It.IsAny<Order>())).Returns(async (Order order) =>
        {
            _shopDbContext.Entry(order).State = EntityState.Modified;
            await _shopDbContext.SaveChangesAsync();
        });
    }

    public void SetupStatusRepository()
    {
        _statusRepository.Setup(x => x.GetStatusById(It.IsAny<int>())).Returns(async (int id) =>
        {
            var status = await _shopDbContext.Statuses.FirstOrDefaultAsync(s => s.Id == id);
            return status;
        });
    }

    public void SetupOrderHistoryRepository()
    {
        _orderHistoryRepository.Setup(x => x.AddHistory(It.IsAny<OrderHistory>())).Returns(async (OrderHistory orderHistory) =>
        {
            _shopDbContext.Add(orderHistory);
            await _shopDbContext.SaveChangesAsync();
            return orderHistory;
        });
    }

    public void SetupUserRepository()
    {
        _userRepository.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(async (int id) =>
        {
            var user = await _shopDbContext.Users.
            FirstOrDefaultAsync(u => u.Id == id);
            return user;
        });

        _userRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(async (string email) =>
        {
            var user = await _shopDbContext.Users.
            FirstOrDefaultAsync(u => u.Email == email);
            return user;
        });
    }

    public void SetupProductRepository()
    {
        _productRepository.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(async (int id) =>
        {
            var product = await _shopDbContext.Products.
            FirstOrDefaultAsync(u => u.Id == id);
            return product;
        });

        _productRepository.Setup(x => x.UpdateProduct(It.IsAny<Product>())).Returns(async (Product product) =>
        {
            _shopDbContext.Entry(product).State = EntityState.Modified;
            await _shopDbContext.SaveChangesAsync();
        });

        _productRepository.Setup(x => x.GetProductsByBrand(It.IsAny<Brand>())).Returns(async (Brand brand) =>
        {
            return await _shopDbContext.Products.Where(p => p.BrandId == brand.Id).ToListAsync();
        });

        _productRepository.Setup(x => x.GetProductsByCategory(It.IsAny<Category>())).Returns(async (Category category) =>
        {
            return await _shopDbContext.Products.Where(p => p.CategoryId == category.Id).ToListAsync();
        });
    }

    public void SetupOrderProductRepository()
    {
        _orderProductRepository.Setup(x => x.AddOrderProduct(It.IsAny<OrderProduct>())).Returns(async (OrderProduct orderProduct) =>
        {
            _shopDbContext.Add(orderProduct);
            await _shopDbContext.SaveChangesAsync();
            return orderProduct;
        });
    }

    public void SetupBrandRepository()
    {
        _brandRepository.Setup(x => x.GetBrandById(It.IsAny<int>())).Returns(async (int id) =>
        {
            var brand = await _shopDbContext.Brands.FirstOrDefaultAsync(s => s.Id == id);
            return brand;
        });
    }

    public void SetupCategoryRepository()
    {
        _categoryRepository.Setup(x => x.GetCategoryById(It.IsAny<int>())).Returns(async (int id) =>
        {
            var category = await _shopDbContext.Categories.FirstOrDefaultAsync(s => s.Id == id);
            return category;
        });
    }
}
