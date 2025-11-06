using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceManagenent.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; } 
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        [NotMapped]
        public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice); 





    }
}
