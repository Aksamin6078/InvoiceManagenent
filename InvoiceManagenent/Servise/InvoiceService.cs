using InvoiceManagenent.Data;
using InvoiceManagenent.DTOs;
using InvoiceManagenent.Models;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagenent.Servise
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto)
        {
            Customer? customer = null;

            if (invoiceDto.CustomerId.HasValue && invoiceDto.CustomerId.Value > 0)
            {
                customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == invoiceDto.CustomerId.Value);
            }

            if (customer == null)
            {
                customer = new Customer
                {
                    CustomerName = invoiceDto.CustomerName,
                    Email = invoiceDto.Email,
                    PhoneNumber = invoiceDto.PhoneNumber
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                invoiceDto.CustomerId = customer.CustomerId;
            }

            var invoice = new Invoice
            {
                CustomerId = customer.CustomerId,
                Date = invoiceDto.Date
            };
            
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();


            foreach (var item in invoiceDto.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} does not exist.");

                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.InvoiceId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                _context.InvoiceItems.Add(invoiceItem);
            }

            await _context.SaveChangesAsync();

            invoiceDto.InvoiceId = invoice.InvoiceId;
            return invoiceDto;
        }

        public async Task<InvoiceDto?> UpdateInvoiceAsync(int id, InvoiceDto invoiceDto)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null) return null;

            Customer? customer = null;

            if (invoiceDto.CustomerId.HasValue && invoiceDto.CustomerId.Value > 0)
            {
                customer = await _context.Customers.FindAsync(invoiceDto.CustomerId.Value);
            }

            if (customer == null)
            {
                if (string.IsNullOrWhiteSpace(invoiceDto.CustomerName))
                    throw new Exception("Customer information missing.");

                customer = new Customer
                {
                    CustomerName = invoiceDto.CustomerName,
                    Email = invoiceDto.Email,
                    PhoneNumber = invoiceDto.PhoneNumber
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                invoiceDto.CustomerId = customer.CustomerId;
            }

            invoice.CustomerId = customer.CustomerId;
            invoice.Date = invoiceDto.Date;

            _context.InvoiceItems.RemoveRange(invoice.Items);
            await _context.SaveChangesAsync();

            foreach (var item in invoiceDto.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} not found.");

                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.InvoiceId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                _context.InvoiceItems.Add(invoiceItem);
            }

            await _context.SaveChangesAsync();
            return invoiceDto;
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null) return false;

            _context.InvoiceItems.RemoveRange(invoice.Items);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync(string? customerName = null)
        {
            var query = _context.Invoices
                .Include(i => i.Items)
                    .ThenInclude(ii => ii.Product)
                .Include(i => i.Customer)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                query = query.Where(i => i.Customer.CustomerName.Contains(customerName));
            }

            var invoices = await query.ToListAsync();

            return invoices.Select(i => new InvoiceDto
            {
                InvoiceId = i.InvoiceId,
                CustomerId = i.CustomerId,
                CustomerName = i.Customer.CustomerName,
                Date = i.Date,
                Items = i.Items.Select(ii => new InvoiceItemDto
                {
                    ProductId = ii.ProductId,
                    ProductName = ii.Product.ProductName,
                    Quantity = ii.Quantity,
                    UnitPrice = ii.UnitPrice
                }).ToList()
            }).ToList();
        }

        public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                    .ThenInclude(ii => ii.Product)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null) return null;

            return new InvoiceDto
            {
                InvoiceId = invoice.InvoiceId,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer.CustomerName,
                Email=invoice.Customer.Email,
                PhoneNumber=invoice.Customer.PhoneNumber,
                Date = invoice.Date,
                Items = invoice.Items.Select(ii => new InvoiceItemDto
                {
                    ProductId = ii.ProductId,
                    ProductName = ii.Product.ProductName,
                    Quantity = ii.Quantity,
                    UnitPrice = ii.UnitPrice
                }).ToList()
            };
        }
    }
}
