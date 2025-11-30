using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.DataOrEntity
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<ProductProductType> ProductProductTypes { get; set; } = new List<ProductProductType>();
    }
}