using InvoiceManagenent.Data;
using InvoiceManagenent.DTOs;
using InvoiceManagenent.Models;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagenent.Servise
{
    public class CategoryServices : ICategorycs
    {
        private readonly AppDbContext _context;

        public CategoryServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                CatagoryName = categoryDto.CatagoryName
            };

             _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            categoryDto.CategoryId = category.CategoryId;

            return categoryDto;

        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
           await _context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CatagoryName = c.CatagoryName
                })
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(string? searchTerm = null)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.CatagoryName.Contains(searchTerm));
            }

            return await query
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CatagoryName = c.CatagoryName
                })
                .ToListAsync();

        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) 
            {
                return null;
            }
            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                CatagoryName = category.CatagoryName
            };


        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var updateCategory = await _context.Categories.FindAsync(id);

            if (updateCategory == null)
            {
                return null;
            }

            updateCategory.CategoryId = categoryDto.CategoryId;
            updateCategory.CatagoryName = categoryDto.CatagoryName;

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = updateCategory.CategoryId,
                CatagoryName = updateCategory.CatagoryName
            };



        }
    }

}
