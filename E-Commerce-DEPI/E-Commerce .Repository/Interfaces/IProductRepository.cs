using E_Commerce.Data.DataOrEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_.Repository.Interfaces
{
    public interface IProductRepository : IGenaricRepository<Product>
    {
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        Task<Product?> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllWithBrandAndTypeAsync();
        Task<Product?> GetByIdWithBrandAndTypeAsync(int id);
        Task<IEnumerable<Product>> GetBestSalesProductsAsync(int count = 8);
        Task<IEnumerable<Product>> GetNewestProductsAsync(int count = 8);
        Task<IEnumerable<Product>> GetProductsByTypeIdAsync(int typeId);
    }
}