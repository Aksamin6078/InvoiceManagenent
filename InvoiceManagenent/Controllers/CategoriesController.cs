using InvoiceManagenent.Data;
using InvoiceManagenent.DTOs;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagenent.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategorycs _categoryService;
        private readonly AppDbContext _context;

        public CategoriesController(ICategorycs categoryService, AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(search);

            var categoriesWithProducts = categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CatagoryName = c.CatagoryName,
                Products = _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == c.CategoryId)
                .ToList()
            });

            ViewBag.Search = search;
            return View(categoriesWithProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }


        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return View(categoryDto);

            await _categoryService.CreateCategoryAsync(categoryDto);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDto categoryDto)
        {
            if (id != categoryDto.CategoryId) return BadRequest();
            if (!ModelState.IsValid) return View(categoryDto);

            var updated = await _categoryService.UpdateCategoryAsync(id, categoryDto);
            if (updated == null) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteCatecory = await _categoryService.DeleteCategoryAsync(id);
            if (!deleteCatecory)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));

        }


    }
}