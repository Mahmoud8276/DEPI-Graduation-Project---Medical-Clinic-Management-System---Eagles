using E_Commerce.Data.DataOrEntity;
using E_Commerce.Services.Model_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E_Commerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductVM>> GetAllProductsAsync();

        Task<ProductVM?> GetProductByIdAsync(int? id);

        Task AddProductAsync(ProductVM productVm);

        Task UpdateProductAsync(UpdateProductVM updateProductVM);

        Task DeleteProductAsync(int? id);

        Task<IReadOnlyList<ProductBrand>> GetAllBrandsAsync();

        Task<IReadOnlyList<ProductType>> GetAllTypesAsync();

        Task<IEnumerable<ProductVM>> GetBestSalesProductsAsync(int count = 8);

        Task<IEnumerable<ProductVM>> GetNewestProductsAsync(int count = 8);

        Task<IEnumerable<ProductVM>> GetProductsByTypeIdAsync(int typeId);

        Task<IEnumerable<ProductVM>> SearchProductsAsync(string query);

        //Task<Product?> GetProductByNameAsync(string name);
    }
}