using InvoiceManagenent.Data;
using InvoiceManagenent.DTOs;
using InvoiceManagenent.Models;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagenent.Servise
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber

                })
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return null;

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
        {
            var customer = new Customer
            {
                CustomerName = customerDto.CustomerName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            customerDto.CustomerId = customer.CustomerId;
            return customerDto;
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(int customerId, CustomerDto customerDto)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return null;

            customer.CustomerName = customerDto.CustomerName;
            customer.Email = customerDto.Email;
            customer.PhoneNumber = customerDto.PhoneNumber;

            await _context.SaveChangesAsync();
            return customerDto;
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
