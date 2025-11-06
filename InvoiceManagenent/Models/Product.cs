using System.ComponentModel.DataAnnotations;

namespace InvoiceManagenent.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } = 0.00m;
        public int Stock { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
