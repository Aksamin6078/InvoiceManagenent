using InvoiceManagenent.DTOs;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceManagenent.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategorycs _categoryService;

        public ProductsController(IProductService productService, ICategorycs categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id) 
        {
            var productWithId =await _productService.GetProductByIdAsync(id);
            return View("Details", productWithId);

        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            var productDto = new ProductDto
            {
                Categories = categories.ToList()
            };

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CatagoryName");

            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CatagoryName");
                return View(productDto);
            }

            await _productService.CreateProductAsync(productDto);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CatagoryName");
            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ProductDto productDto)
        {
          if(id != productDto.ProductId)
            {
                return BadRequest();
            }

          if(!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CatagoryName");
                return View(productDto);
            }

          var updatedProduct = await _productService.UpdateProductAsync(id,productDto);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
