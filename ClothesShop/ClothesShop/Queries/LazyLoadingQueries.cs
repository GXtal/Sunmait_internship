using ClothesShop.Entities;
using ClothesShop.Queries.ReturnTypes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ClothesShop.Queries
{
    public static class LazyLoadingQueries
    {
        public static async Task<List<ProductInfo>> GetProductsByBrand(int brandId, ShopdbContext context)
        {
            var query = context.Products
                .Where(p => p.BrandId == brandId);
            var products = await query.ToListAsync();

            var productInfos = products
                .Select(p => new ProductInfo
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    ProductDescription = p.Description,
                    ProductQuantity = p.Quantity,
                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,
                })
                .ToList();

            return productInfos;
        }

        public static async Task<List<BrandWithProductCount>> GetBrandsWithProductCount(ShopdbContext context)
        {
            var query = context.Brands;
            var brands = await query.ToListAsync();
            
            var brandsWithProductCount = brands
                .Select(b => new BrandWithProductCount
                {
                    BrandId = b.Id,
                    BrandName = b.Name,
                    ProductsCount = b.Products.Count()
                })
                .OrderByDescending(b => b.ProductsCount)
                .ToList();

            return brandsWithProductCount;
        }

        public static async Task<List<ProductInfo>> GetProductsByCategoryAndSection(int categoryId, int sectionId, ShopdbContext context)
        {
            var query = context.Products
                .Where(p => p.CategoryId == categoryId && p.Category.CategoriesSections.Any(s => s.SectionId == sectionId));
            var products = await query.ToListAsync();

            var productsInfo = products
                .Select(p => new ProductInfo
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    ProductDescription = p.Description,
                    ProductQuantity = p.Quantity,
                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,
                })
                .ToList();

            return productsInfo;
        }

        public static async Task<List<CompletedOrder>> GetCompletedOrdersByProduct(int productId, ShopdbContext context)
        {
            var query = context.Orders
                .Where(o => o.OrderHistories.Any(oh => oh.StatusId == 3) && o.OrdersProducts.Any(op => op.ProductId == productId));
            var orders = await query.ToListAsync();

            var completedOrders = orders
                .Select(o => new CompletedOrder
                {
                    OrderId = o.Id,
                    UserId = o.UserId,
                    TotalOrderCost = o.TotalCost,
                    ProductCount = o.OrdersProducts.FirstOrDefault(op => op.ProductId == productId).Count,
                    CompleteTime = o.OrderHistories.FirstOrDefault(oh => oh.StatusId == 3).SetTime
                })
                .OrderByDescending(oc => oc.CompleteTime)
                .ToList();

            return completedOrders;               
        }

        public static async Task<List<ProductReview>> GetProductReviews(int productId, ShopdbContext context)
        {
            var query = context.Reviews
                .Where(r => r.ProductId == productId);
            var reviews = await query.ToListAsync();

            var productReviews = reviews
                .Select(r => new ProductReview
                {
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CommentatorName = $"{r.User.Name} {r.User.Surname}",
                    CommentatorEmail = r.User.Email
                })
                .ToList();

            return productReviews;
        }
    }
}
