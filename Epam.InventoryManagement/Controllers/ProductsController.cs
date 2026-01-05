using Epam.InventoryManagement.Application.DTOs;
using Epam.InventoryManagement.Application.Interfaces;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace Epam.InventoryManagement.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Product data is required");

            var id = await _service.AddProductAsync(dto);
            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
        {
            if (dto == null || dto.ProductId <= 0)
                return BadRequest("Invalid product data");

            var result = await _service.UpdateProductAsync(dto);
            return result ? Ok(result) : NotFound();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid id");

            var product = await _service.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllProductsAsync());

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword is required");

            return Ok(await _service.SearchProductsAsync(keyword));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid id");

            var result = await _service.DeleteProductAsync(id);
            return result ? Ok(result) : NotFound();
        }
    }
}
