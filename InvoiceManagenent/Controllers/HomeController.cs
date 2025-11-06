using System.Diagnostics;
using InvoiceManagenent.Models;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagenent.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategorycs _categoryService;
        private readonly IInvoiceService _invoiceService;

        public HomeController(IProductService productService, ICategorycs categoryService, IInvoiceService invoiceService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();
            var invoices = await _invoiceService.GetAllInvoicesAsync();

            ViewBag.TotalProducts = products.Count();
            ViewBag.TotalCategories = categories.Count();
            ViewBag.LowStockProducts = products.Count(p => p.Stock < 5);

            ViewBag.CategoryNames = categories.Select(c => c.CatagoryName).ToList();
            ViewBag.ProductCounts = categories.Select(c => products.Count(p => p.CategoryId == c.CategoryId)).ToList();

            ViewBag.TotalInvoices = invoices.Count();
            ViewBag.TotalRevenue = invoices.Sum(i => i.Items.Sum(x => x.Quantity * x.UnitPrice));


            ViewBag.MonthlySalesLabels = invoices
                .GroupBy(i => i.Date.ToString("MMM yyyy"))
                .Select(g => g.Key)
                .ToList();

            ViewBag.MonthlySalesData = invoices
                .GroupBy(i => i.Date.ToString("MMM yyyy"))
                .Select(g => g.Sum(i => i.Items.Sum(x => x.Quantity * x.UnitPrice)))
                .ToList();

            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
