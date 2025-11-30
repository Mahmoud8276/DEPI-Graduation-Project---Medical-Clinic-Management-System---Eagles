using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.DataOrEntity
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int StockQuantity { get; set; }

        public string PictureUrl { get; set; }

        public decimal? Discount { get; set; }

        public ICollection<ProductProductType> ProductProductTypes { get; set; } = new List<ProductProductType>();
        public DateTime CreatedAt { get; set; }
    }
}