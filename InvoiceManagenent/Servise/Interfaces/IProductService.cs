using InvoiceManagenent.DTOs;

namespace InvoiceManagenent.Servise.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task<ProductDto?> UpdateProductAsync(int productId, ProductDto productDto);
        Task<bool> DeleteProductAsync(int productId);


    }
}
