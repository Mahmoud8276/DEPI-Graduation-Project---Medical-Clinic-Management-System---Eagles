using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Data.DataOrEntity
{
    public class ProductProductType
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}