using Application.Inputs;
using Application.Outputs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBrandRepository
    {
        public bool DeleteBrand(int id);
        public BrandOutput GetBrandById(int id);
        public IEnumerable<BrandOutput> GetBrands();
        public bool InsertBrand(BrandInput newBrand);
        public void Save();
        public bool UpdateBrand(int id, BrandInput newBrand);
    }
}
