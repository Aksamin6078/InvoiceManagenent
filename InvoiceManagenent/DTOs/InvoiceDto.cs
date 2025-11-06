namespace InvoiceManagenent.DTOs
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }  
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public DateTime Date { get; set; } = DateTime.Now; 
        public List<InvoiceItemDto> Items { get; set; } = new();

        public decimal Total => Items.Sum(i => i.Total);

    }
}
