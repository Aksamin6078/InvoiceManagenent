using InvoiceManagenent.DTOs;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceManagenent.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public ReportsController(
            IInvoiceService invoiceService,
            IProductService productService,
            ICustomerService customerService)
        {
            _invoiceService = invoiceService;
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            var products = await _productService.GetAllProductsAsync();
            var customers = await _customerService.GetAllCustomersAsync();

            var totalRevenue = invoices.Sum(i => i.Items.Sum(it => it.Quantity * it.UnitPrice));

            var monthlySales = invoices
                .GroupBy(i => i.Date.ToString("MMM yyyy"))
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(i => i.Items.Sum(it => it.Quantity * it.UnitPrice))
                })
                .ToList();

            var topProducts = invoices
                .SelectMany(i => i.Items)
                .GroupBy(i => i.ProductName)
                .Select(g => new
                {
                    Product = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            var lowStock = products.Where(p => p.Stock < 5).ToList();

            var topCustomers = invoices
                .Where(i => !string.IsNullOrEmpty(i.CustomerName))
                .GroupBy(i => i.CustomerName)
                .Select(g => new
                {
                    Customer = g.Key,
                    TotalSpent = g.Sum(i => i.Items.Sum(it => it.Quantity * it.UnitPrice))
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(5)
                .ToList();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.MonthlySalesLabels = monthlySales.Select(x => x.Month).ToList();
            ViewBag.MonthlySalesValues = monthlySales.Select(x => x.Total).ToList();
            ViewBag.TopProducts = topProducts;
            ViewBag.LowStock = lowStock;
            ViewBag.TopCustomers = topCustomers;

            return View();
        }
    }
}
