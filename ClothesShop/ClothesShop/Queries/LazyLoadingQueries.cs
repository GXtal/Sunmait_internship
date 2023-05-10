using ClothesShop.Entities;
using ClothesShop.Queries.ReturnTypes;
using Microsoft.EntityFrameworkCore;

namespace ClothesShop.Queries
{
    public static class LazyLoadingQueries
    {
        public static async Task<List<ProductInfo>> GetProductsByBrand(int brandId, ShopdbContext context)
        {
            var query = context.Products
                .Where(p => p.BrandId == brandId);
            var products = await query.ToListAsync();

            return products.Select(p => new ProductInfo
            {
                ProductId = p.Id,
                ProductName = p.Name,
                ProductPrice = p.Price,
                ProductDescription = p.Description,
                ProductQuantity = p.Quantity,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
            }).ToList();
        }

        public static async Task<List<BrandWithProductCount>> GetBrandsWithProductCount(ShopdbContext context)
        {
            var query = context.Brands
                .Select(b => new BrandWithProductCount
                {
                    BrandId = b.Id,
                    BrandName = b.Name,
                    ProductsCount = b.Products.Count()
                })
                .OrderByDescending(b => b.ProductsCount);
            return await query.ToListAsync();
        }

        public static async Task<List<ProductInfo>> GetProductsByCategoryAndSection(int categoryId, int sectionId, ShopdbContext context)
        {
            var query = context.Products
                .Where(p => p.CategoryId == categoryId && p.Category.CategoriesSections.Any(s => s.SectionId == sectionId))
                .Select(p => new ProductInfo
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    ProductDescription = p.Description,
                    ProductQuantity = p.Quantity,
                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,
                });                

            return await query.ToListAsync();
        }

        public static async Task<List<CompletedOrder>> GetCompletedOrdersByProduct(int productId, ShopdbContext context)
        {
            var query = context.Orders
                .Where(o => o.OrderHistories.Any(oh => oh.StatusId == 3) && o.OrdersProducts.Any(op => op.ProductId == productId))
                .Select(o => new CompletedOrder
                {
                    OrderId = o.Id,
                    UserId = o.UserId,
                    TotalOrderCost = o.TotalCost,
                    ProductCount = o.OrdersProducts.FirstOrDefault(op => op.ProductId == productId).Count,
                    CompleteTime = o.OrderHistories.FirstOrDefault(oh => oh.StatusId == 3).SetTime
                })
                .OrderByDescending(oc => oc.CompleteTime);

            return await query.ToListAsync();               
        }

        public static async Task<List<ProductReview>> GetProductReviewsAsync(int productId, ShopdbContext context)
        {
            var query = context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => new ProductReview
                {
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CommentatorName = $"{r.User.Name} {r.User.Surname}",
                    CommentatorEmail = r.User.Email
                });                

            return await query.ToListAsync();
        }
    }
}
