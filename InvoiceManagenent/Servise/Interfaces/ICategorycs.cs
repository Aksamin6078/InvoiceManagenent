using InvoiceManagenent.DTOs;

namespace InvoiceManagenent.Servise.Interfaces
{
    public interface ICategorycs
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(string? searchTerm = null);
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
        Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);

    }
}
