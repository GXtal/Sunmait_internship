using ClothesShop.Entities;
using ClothesShop.Queries;
using ClothesShop.Queries.ReturnTypes;
using Microsoft.EntityFrameworkCore;

namespace ClothesShop
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Checking LazyLoadingQueries.GetProductsByBrand");
            using(var dbContext = new ShopDbContext())
            {
                var productsByBrand = await LazyLoadingQueries.GetProductsByBrand(1, dbContext);
                foreach(var product in productsByBrand)
                {
                    Console.WriteLine(product);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetProductsByBrand");
            using(var dbContext = new ShopDbContext())
            {
                var productsByBrand = await EagerLoadingQueries.GetProductsByBrand(1, dbContext);
                foreach(var product in productsByBrand)
                {
                    Console.WriteLine(product);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetBrandsWithProductCount");
            using(var dbContext = new ShopDbContext())
            {
                var brandsWithProductCount = await LazyLoadingQueries.GetBrandsWithProductCount(dbContext);
                foreach(var brandWithProductCount in brandsWithProductCount)
                {
                    Console.WriteLine(brandWithProductCount);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetBrandsWithProductCount");
            using(var dbContext = new ShopDbContext())
            {
                var brandsWithProductCount = await EagerLoadingQueries.GetBrandsWithProductCount(dbContext);
                foreach(var brandWithProductCount in brandsWithProductCount)
                {
                    Console.WriteLine(brandWithProductCount);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetProductsByCategoryAndSection");
            using(var dbContext = new ShopDbContext())
            {
                var productsByCategoryAndSection = await EagerLoadingQueries.GetProductsByCategoryAndSection(2, 1, dbContext);
                foreach(var product in productsByCategoryAndSection)
                {
                    Console.WriteLine(product);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetProductsByCategoryAndSection");
            using(var dbContext = new ShopDbContext())
            {
                var productsByCategoryAndSection = await LazyLoadingQueries.GetProductsByCategoryAndSection(2, 1, dbContext);
                foreach(var product in productsByCategoryAndSection)
                {
                    Console.WriteLine(product);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetCompletedOrdersByProduct");
            using(var dbContext = new ShopDbContext())
            {
                var completeOrders = await LazyLoadingQueries.GetCompletedOrdersByProduct(1, dbContext);
                foreach(var order in completeOrders)
                {
                    Console.WriteLine(order);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetCompletedOrdersByProduct");
            using(var dbContext = new ShopDbContext())
            {
                var completeOrders = await EagerLoadingQueries.GetCompletedOrdersByProduct(1, dbContext);
                foreach(var order in completeOrders)
                {
                    Console.WriteLine(order);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetProductReviews");
            using(var dbContext = new ShopDbContext())
            {
                var productReviews = await LazyLoadingQueries.GetProductReviews(3, dbContext);
                foreach(var review in productReviews)
                {
                    Console.WriteLine(review);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetProductReviews");
            using(var dbContext = new ShopDbContext())
            {
                var productReviews = await EagerLoadingQueries.GetProductReviews(3, dbContext);
                foreach(var review in productReviews)
                {
                    Console.WriteLine(review);
                }
            }

            // Explicit tracking
            Console.WriteLine("Before update");
            using(var context = new ShopDbContext())
            {
                var allProducts = await context.Products.ToListAsync();
                foreach(var product in allProducts)
                {
                    Console.WriteLine($"{product.Id}: {product.Quantity}");
                }
            }

            Product changingProduct;
            using(var context = new ShopDbContext())
            {
                changingProduct = context.Products.Single(p => p.Id == 1);
            }

            changingProduct.Quantity += 5;

            using(var context = new ShopDbContext())
            {
                context.Update(changingProduct);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("After update");
            using(var context = new ShopDbContext())
            {
                var allProducts = await context.Products.ToListAsync();
                foreach(var product in allProducts)
                {
                    Console.WriteLine($"{product.Id}: {product.Quantity}");
                }
            }
        }
    }
}