using Epam.InventoryManagement.Application.DTOs;
using Epam.InventoryManagement.Application.Interfaces;
using Epam.InventoryManagement.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Epam.InventoryManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> AddProductAsync(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

            return await _repo.AddAsync(product);
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto dto)
        {
            var product = new Product
            {
                ProductId = dto.ProductId,
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

            return await _repo.UpdateAsync(product);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            return product == null ? null : Map(product);
        }

        public async Task<List<ProductResponseDto>> GetAllProductsAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(Map).ToList();
        }

        public async Task<List<ProductResponseDto>> SearchProductsAsync(string keyword)
        {
            var list = await _repo.SearchAsync(keyword);
            return list.Select(Map).ToList();
        }

        public Task<bool> DeleteProductAsync(int id)
            => _repo.DeleteAsync(id);

        private static ProductResponseDto Map(Product p) => new()
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Category = p.Category,
            Price = p.Price,
            Quantity = p.Quantity
        };
    }
}

