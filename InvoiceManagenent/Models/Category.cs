using System.ComponentModel.DataAnnotations;

namespace InvoiceManagenent.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(200)]
        public string CatagoryName { get; set; }=string.Empty;

        public ICollection<Product> Products { get; set; }= new List<Product>();

    }
}