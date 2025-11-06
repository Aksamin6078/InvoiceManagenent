using System.ComponentModel.DataAnnotations;

namespace InvoiceManagenent.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(200)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }


    }
}
