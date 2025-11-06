using InvoiceManagenent.DTOs;

namespace InvoiceManagenent.Servise.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync(string? customerName = null);
        Task<InvoiceDto?> GetInvoiceByIdAsync(int id);
        Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto);
        Task<InvoiceDto?> UpdateInvoiceAsync(int id, InvoiceDto invoiceDto);
        Task<bool> DeleteInvoiceAsync(int id);

    }
}
