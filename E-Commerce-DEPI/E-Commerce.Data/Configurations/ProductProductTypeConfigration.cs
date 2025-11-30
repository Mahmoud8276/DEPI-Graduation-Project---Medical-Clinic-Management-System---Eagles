using E_Commerce.Data.DataOrEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.Configurations
{
    public class ProductProductTypeConfiguration : IEntityTypeConfiguration<ProductProductType>
    {
        public void Configure(EntityTypeBuilder<ProductProductType> builder)
        {
            builder.HasKey(ppt => new { ppt.ProductId, ppt.ProductTypeId });
        }
    }
}