using AutoMapper;
using E_Commerce.Data.DataOrEntity;
using E_Commerce.Services.Model_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Services.Mapping_Product
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductVM, Product>()
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ProductProductTypes, opt => opt.MapFrom(src => src.ProductTypeIds.Select(id => new ProductProductType { ProductTypeId = id })))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));

            CreateMap<UpdateProductVM, Product>()
                .ForMember(dest => dest.ProductProductTypes, opt => opt.MapFrom(src => src.ProductTypeIds.Select(id => new ProductProductType { ProductTypeId = id })))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));

            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.ProductTypeIds, opt => opt.MapFrom(src => src.ProductProductTypes.Select(pt => pt.ProductTypeId)))
                .ForMember(dest => dest.TypeNames, opt => opt.MapFrom(src => src.ProductProductTypes.Select(pt => pt.ProductType.Name)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));
        }
    }
}