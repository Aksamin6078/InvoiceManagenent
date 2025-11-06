using InvoiceManagenent.DTOs;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceManagenent.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public InvoicesController(IInvoiceService invoiceService, ICustomerService customerService, IProductService productService)
        {
            _invoiceService = invoiceService;
            _customerService = customerService;
            _productService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? customerName)
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync(customerName);
            ViewData["CustomerName"] = customerName; 
            return View(invoices);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound();
            return View(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var products = await _productService.GetAllProductsAsync();

            ViewBag.Customers = new SelectList(customers, "CustomerId", "CustomerName");

            ViewBag.Products = products.Select(p => new
            {
                value = p.ProductId,
                text = $"{p.ProductName} (${p.Price})",
                price = p.Price
            }).ToList();

            return View(new InvoiceDto());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceDto invoiceDto)
        {
            if (!ModelState.IsValid)
            {
                var customers = await _customerService.GetAllCustomersAsync();
                var products = await _productService.GetAllProductsAsync();

                ViewBag.Customers = new SelectList(customers, "CustomerId", "CustomerName");
                ViewBag.Products = products.Select(p => new
                {
                    value = p.ProductId,
                    text = $"{p.ProductName} (${p.Price})",
                    price = p.Price
                }).ToList();

                return View(invoiceDto);
            }

            if (invoiceDto.Items == null || !invoiceDto.Items.Any())
            {
                ModelState.AddModelError("", "Please add at least one invoice item.");
                return View(invoiceDto);
            }

            await _invoiceService.CreateInvoiceAsync(invoiceDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound();

            var customers = await _customerService.GetAllCustomersAsync();
            var products = await _productService.GetAllProductsAsync();

            ViewBag.Customers = new SelectList(customers, "CustomerId", "CustomerName");
            ViewBag.ProductSelectList = new SelectList(products, "ProductId", "ProductName");

            ViewBag.Products = products;

            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvoiceDto invoiceDto)
        {
            if (id != invoiceDto.InvoiceId) return BadRequest();

            if (!ModelState.IsValid)
            {
                var customers = await _customerService.GetAllCustomersAsync();
                var products = await _productService.GetAllProductsAsync();

                ViewBag.Customers = new SelectList(customers, "CustomerId", "CustomerName");
                ViewBag.ProductSelectList = new SelectList(products, "ProductId", "ProductName");
                ViewBag.Products = products;

                return View(invoiceDto);
            }


            var updated = await _invoiceService.UpdateInvoiceAsync(id, invoiceDto);
            if (updated == null) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
