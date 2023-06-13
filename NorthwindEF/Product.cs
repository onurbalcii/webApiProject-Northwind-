using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindEF
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("units_in_stock")]
        public int UnitStock { get; set; }

        [Column("unit_price")]
        public float UnitPrice { get; set; }
    }
}
