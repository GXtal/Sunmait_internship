using Application.Inputs;
using Application.Interfaces;
using Application.Outputs;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ShopDbContext _dbContext;

        public BrandRepository(ShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BrandOutput GetBrandById(int id)
        {
            var brand=_dbContext.Brands.Find(id);
            if(brand == null) { return null; }
            return new BrandOutput() { Id = brand.Id, Name = brand.Name };
        }

        public IEnumerable<BrandOutput> GetBrands()
        {
            var allBrands = _dbContext.Brands.ToList();
            var allBrndsSimplified = new List<BrandOutput>();
            foreach(var brand in allBrands)
            {
                allBrndsSimplified.Add(new BrandOutput() { Id = brand.Id, Name= brand.Name });
            }
            return allBrndsSimplified;
        }

        public bool InsertBrand(BrandInput newBrand)
        {
            var brand = new Brand() { Id = 0, Name = newBrand.Name };
            if(_dbContext.Brands.Any(b=>b.Name==newBrand.Name))
            {
                return false;
            }
            _dbContext.Add(brand);
            Save();
            return true;
        }

        public bool UpdateBrand(int id, BrandInput newBrand)
        {
            var brand = new Brand() { Id = id, Name = newBrand.Name};
            if(_dbContext.Brands.Any(b => (b.Name == newBrand.Name)&&(b.Id!=brand.Id)))
            {
                return false;
            }
            _dbContext.Entry(brand).State = EntityState.Modified;
            Save();
            return true;
        }

        public bool DeleteBrand(int productId)
        {
            var brand = _dbContext.Brands.Find(productId);
            if(brand == null)
            {
                return false;
            }
            _dbContext.Brands.Remove(brand);
            Save();
            return true;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
