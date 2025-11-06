using InvoiceManagenent.Data;
using InvoiceManagenent.DTOs;
using InvoiceManagenent.Models;
using InvoiceManagenent.Servise.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagenent.Servise
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {

            var products = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking() 
                .ToListAsync(); 

            var productDtos = products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.CatagoryName ?? ""
            }).ToList();

            return productDtos;
        }


        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(c => c.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return null; 

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId=product.CategoryId,
                CategoryName = product.Category.CatagoryName,
            };
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var createProduct = new Models.Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
            };

            _context.Products.Add(createProduct);
            await _context.SaveChangesAsync();

            productDto.ProductId = createProduct.ProductId;
            return productDto;
        }


        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.Include(c=>c.Category)
                .FirstOrDefaultAsync(p=>p.ProductId == productId);
            if (product == null)
            {
                return false; 
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<ProductDto> UpdateProductAsync(int productId, ProductDto productDto)
        {
            var updateProduct = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (updateProduct == null)
            {
                return null;
            }

            updateProduct.ProductName = productDto.ProductName;
            updateProduct.Price = productDto.Price;
            updateProduct.Stock = productDto.Stock;
            updateProduct.CategoryId = productDto.CategoryId;

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                ProductId = updateProduct.ProductId,
                ProductName = updateProduct.ProductName,
                Price = updateProduct.Price,
                Stock = updateProduct.Stock,
                CategoryId = updateProduct.CategoryId,
                CategoryName = updateProduct.Category?.CatagoryName ?? string.Empty
            };
        }

    }
}
