using E_Commerce.Data.DataOrEntity;
using E_Commerce_.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace E_Commerce_.Repository.Repository
{
    public class ProductRepository : GenaricRepository<Product>, IProductRepository
    {
        private readonly E_CommerceDbContext _context;

        public ProductRepository(E_CommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllWithBrandAndTypeAsync()
        {
            var result = await _context.products
                .Include(p => p.ProductProductTypes)
                    .ThenInclude(pt => pt.ProductType)
                .ToListAsync();
            return result;
        }

        public async Task<Product?> GetByIdWithBrandAndTypeAsync(int id)
        {
            var result = await _context.products
                .Include(p => p.ProductProductTypes)
                    .ThenInclude(pt => pt.ProductType)
                                                .FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            var result = await _context.productBrands.ToListAsync();
            return result;

        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await _context.products
                .Include(p => p.ProductProductTypes)
                    .ThenInclude(pt => pt.ProductType)
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            var result = await _context.productTypes.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Product>> GetBestSalesProductsAsync(int count = 8)
        {
            return await _context.products
                .Include(p => p.ProductProductTypes)
                    .ThenInclude(pt => pt.ProductType)
                .Where(p => p.Discount.HasValue && p.Discount.Value > 0)
                .OrderByDescending(p => p.Discount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetNewestProductsAsync(int count = 8)
        {
            return await _context.products
                .Include(p => p.ProductProductTypes)
                    .ThenInclude(pt => pt.ProductType)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByTypeIdAsync(int typeId)
        {
            return await _context.products
                .Include(p => p.ProductProductTypes)
                    .ThenInclude(pt => pt.ProductType)
                .Where(p => p.ProductProductTypes.Any(pt => pt.ProductTypeId == typeId))
                .ToListAsync();
        }
    }
}