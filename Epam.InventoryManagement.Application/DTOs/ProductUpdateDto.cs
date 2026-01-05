using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.InventoryManagement.Application.DTOs
{
    public class ProductUpdateDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
