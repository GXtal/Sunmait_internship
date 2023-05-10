using ClothesShop.Queries;
using Microsoft.EntityFrameworkCore;

namespace ClothesShop
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (var dbContext = new ShopdbContext())
            {
                var allProd = await LazyLoadingQueries.GetProductsByBrand(1, dbContext);
                foreach (var item in allProd)
                {
                    Console.WriteLine(item);
                }
            }
            using (var dbContext = new ShopdbContext())
            {
                var allProd = await EagerLoadingQueries.GetProductsByBrand(1, dbContext);
                foreach (var item in allProd)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Hello, World!");

            using (var dbContext = new ShopdbContext())
            {
                var allBrands = await LazyLoadingQueries.GetBrandsWithProductCount(dbContext);
                foreach (var item in allBrands)
                {
                    Console.WriteLine(item);
                }
            }

            using (var dbContext = new ShopdbContext())
            {
                var allBrands = await EagerLoadingQueries.GetBrandsWithProductCount(dbContext);
                foreach (var item in allBrands)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Hello, World!");

            using (var dbContext = new ShopdbContext())
            {
                var prods = await EagerLoadingQueries.GetProductsByCategoryAndSection(2, 1, dbContext);
                foreach (var item in prods)
                {
                    Console.WriteLine(item);
                }
            }
            using (var dbContext = new ShopdbContext())
            {
                var prods = await LazyLoadingQueries.GetProductsByCategoryAndSection(2, 1, dbContext);
                foreach (var item in prods)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}