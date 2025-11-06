using System.ComponentModel.DataAnnotations;

namespace InvoiceManagenent.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string CustomerName { get; set; } = string.Empty;
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }  



    }
}
