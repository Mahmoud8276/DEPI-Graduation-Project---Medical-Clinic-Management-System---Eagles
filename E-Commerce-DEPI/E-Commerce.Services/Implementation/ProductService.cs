using AutoMapper;
using Azure;
using E_Commerce.Data.DataOrEntity;
using E_Commerce.Services.FormFiles;
using E_Commerce.Services.Interfaces;
using E_Commerce.Services.Model_View;
using E_Commerce_.Repository.Interfaces;
using E_Commerce_.Repository.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IFileService fileService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetAllBrandsAsync()
        {
            return await _unitOfWork.ProductRepository.GetProductBrandsAsync();
        }

        public async Task<IEnumerable<ProductVM>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithBrandAndTypeAsync();
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IReadOnlyList<ProductType>> GetAllTypesAsync()
        {
            return await _unitOfWork.ProductRepository.GetProductTypesAsync();
        }

        public async Task<ProductVM?> GetProductByIdAsync(int? id)
        {
            if (id is null || id <= 0)
                return null;  // بدلاً من رمي استثناء

            var product = await _unitOfWork.ProductRepository.GetByIdWithBrandAndTypeAsync(id.Value);
            if (product == null)
                return null;

            return _mapper.Map<ProductVM>(product);
        }

        public async Task UpdateProductAsync(UpdateProductVM updateProductVM)
        {
            if (updateProductVM.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(updateProductVM.Id), "Invalid Product Id");

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(updateProductVM.Id);
            if (product is null)
                throw new ArgumentException("Product not found");

            _mapper.Map(updateProductVM, product);

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddProductAsync(ProductVM productVm)
        {
            if (productVm is null)
                throw new ArgumentNullException(nameof(productVm));

            if (string.IsNullOrWhiteSpace(productVm.Name))
                throw new ArgumentException("Product Name is required");

            if (productVm.Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(productVm.Price), "Price must be greater than 0");

            if (productVm.ProductTypeIds == null || !productVm.ProductTypeIds.Any())
                throw new ArgumentOutOfRangeException(nameof(productVm.ProductTypeIds), "At least one Type must be selected");

            string? imageUrl = null;

            if (productVm.Image is not null)
            {
                imageUrl = await _fileService.UploadFileAsync(productVm.Image);
                if (string.IsNullOrEmpty(imageUrl))
                    throw new Exception("Image upload failed");
            }

            var product = _mapper.Map<Product>(productVm);
            product.PictureUrl = imageUrl;
            product.CreatedAt = DateTime.UtcNow;

            // Remove duplicates and ensure only valid electronics types are added
            var allTypes = await _unitOfWork.ProductRepository.GetProductTypesAsync();
            var allowedTypeNames = new[] { "phones", "computers", "laptops", "smart watches", "camera", "headphones", "gaming", "accessories", "printers", "pc", "televisions" };
            var allowedTypeIds = allTypes.Where(t => allowedTypeNames.Contains(t.Name.ToLower())).Select(t => t.Id).ToHashSet();
            product.ProductProductTypes = product.ProductProductTypes.Where(pt => allowedTypeIds.Contains(pt.ProductTypeId)).GroupBy(pt => pt.ProductTypeId).Select(g => g.First()).ToList();

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProductAsync(int? id)
        {
            if (id is null or <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid Product Id");

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id.Value);
            if (product is null)
                throw new KeyNotFoundException($"Product with ID {id} was not found.");

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ProductVM>> GetBestSalesProductsAsync(int count = 8)
        {
            var products = await _unitOfWork.ProductRepository.GetBestSalesProductsAsync(count);
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IEnumerable<ProductVM>> GetNewestProductsAsync(int count = 8)
        {
            var products = await _unitOfWork.ProductRepository.GetNewestProductsAsync(count);
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IEnumerable<ProductVM>> GetProductsByTypeIdAsync(int typeId)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByTypeIdAsync(typeId);
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IEnumerable<ProductVM>> SearchProductsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<ProductVM>();
            query = query.ToLower();
            var products = await _unitOfWork.ProductRepository.GetAllWithBrandAndTypeAsync();
            var filtered = products.Where(p =>
                (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(query)) ||
                (p.ProductProductTypes != null && p.ProductProductTypes.Any(pt => pt.ProductType.Name.ToLower().Contains(query))) ||
                (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(query))
            );
            return _mapper.Map<IEnumerable<ProductVM>>(filtered);
        }
    }
}