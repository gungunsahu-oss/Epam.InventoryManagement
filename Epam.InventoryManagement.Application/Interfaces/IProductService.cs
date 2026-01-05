using Epam.InventoryManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.InventoryManagement.Application.DTOs;
using global::Epam.InventoryManagement.Application.DTOs;


namespace Epam.InventoryManagement.Application.Interfaces
    {
        public interface IProductService
        {
            Task<int> AddProductAsync(ProductCreateDto dto);
            Task<bool> UpdateProductAsync(ProductUpdateDto dto);
            Task<bool> DeleteProductAsync(int id);
            Task<ProductResponseDto?> GetProductByIdAsync(int id);
            Task<List<ProductResponseDto>> GetAllProductsAsync();
            Task<List<ProductResponseDto>> SearchProductsAsync(string keyword);
        }
    }


