namespace InvoiceManagenent.DTOs
{
    public class InvoiceItemDto
    {
        public int? InvoiceItemId { get; set; } 
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;

    }
}
