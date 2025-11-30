using E_Commerce.Data.DataOrEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Configurations
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.HasKey(x => x.Id);

            builder
                .HasMany(pt => pt.ProductProductTypes)
                .WithOne(ppt => ppt.ProductType)
                .HasForeignKey(ppt => ppt.ProductTypeId);
        }
    }
}