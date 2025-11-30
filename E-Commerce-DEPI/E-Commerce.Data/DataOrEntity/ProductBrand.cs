using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.DataOrEntity
{
    public class ProductBrand:BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; }=new List<Product>();


    }
}
