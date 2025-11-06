using InvoiceManagenent.Models;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManagenent.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(200)]
        public string CatagoryName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
