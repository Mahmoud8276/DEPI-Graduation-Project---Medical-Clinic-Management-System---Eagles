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
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PictureUrl).IsRequired();
            builder.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)");

            builder
                .HasMany(p => p.ProductProductTypes)
                .WithOne(pt => pt.Product)
                .HasForeignKey(pt => pt.ProductId);
        }
    }
}