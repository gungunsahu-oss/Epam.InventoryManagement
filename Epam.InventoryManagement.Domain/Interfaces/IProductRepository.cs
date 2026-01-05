using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.InventoryManagement.Domain
{
    public interface IProductRepository
    {
        Task<int> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int productId);
        Task<Product?> GetByIdAsync(int productId);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> SearchAsync(string keyword);
    }
}
