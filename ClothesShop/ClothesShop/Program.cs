using ClothesShop.Queries;
using Microsoft.EntityFrameworkCore;

namespace ClothesShop
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Checking LazyLoadingQueries.GetProductsByBrand");
            using (var dbContext = new ShopdbContext())
            {
                var allProd = await LazyLoadingQueries.GetProductsByBrand(1, dbContext);
                foreach (var item in allProd)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetProductsByBrand");
            using (var dbContext = new ShopdbContext())
            {
                var allProd = await EagerLoadingQueries.GetProductsByBrand(1, dbContext);
                foreach (var item in allProd)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetBrandsWithProductCount");
            using (var dbContext = new ShopdbContext())
            {
                var allBrands = await LazyLoadingQueries.GetBrandsWithProductCount(dbContext);
                foreach (var item in allBrands)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetBrandsWithProductCount");
            using (var dbContext = new ShopdbContext())
            {
                var allBrands = await EagerLoadingQueries.GetBrandsWithProductCount(dbContext);
                foreach (var item in allBrands)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetProductsByCategoryAndSection");
            using (var dbContext = new ShopdbContext())
            {
                var prods = await EagerLoadingQueries.GetProductsByCategoryAndSection(2, 1, dbContext);
                foreach (var item in prods)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetProductsByCategoryAndSection");
            using (var dbContext = new ShopdbContext())
            {
                var prods = await LazyLoadingQueries.GetProductsByCategoryAndSection(2, 1, dbContext);
                foreach (var item in prods)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetCompletedOrdersByProduct");
            using (var dbContext = new ShopdbContext())
            {
                var orders = await LazyLoadingQueries.GetCompletedOrdersByProduct(1, dbContext);
                foreach (var item in orders)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetCompletedOrdersByProduct");
            using (var dbContext = new ShopdbContext())
            {
                var orders = await EagerLoadingQueries.GetCompletedOrdersByProduct(1, dbContext);
                foreach (var item in orders)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking LazyLoadingQueries.GetCompletedOrdersByProduct");
            using (var dbContext = new ShopdbContext())
            {
                var reviews = await LazyLoadingQueries.GetProductReviews(3,dbContext);
                foreach (var item in reviews)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Checking EagerLoadingQueries.GetCompletedOrdersByProduct");
            using (var dbContext = new ShopdbContext())
            {
                var reviews = await EagerLoadingQueries.GetProductReviews(3, dbContext);
                foreach (var item in reviews)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}