using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.ViewModels.Input;
using StockManagement.Api.ViewModels.Output;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace StockManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductOutputViewModel>> GetById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var productOutputViewModel = new ProductOutputViewModel
            {
                Id = product.Id,
                Name = product.Name,
                CostPrice = product.CostPrice
            };

            return Ok(productOutputViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductInputViewModel productInputViewModel)
        {
            var product = new Product(productInputViewModel.Name, productInputViewModel.CostPrice);

            await _productRepository.SaveAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProductInputViewModel productInputViewModel)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            product.Update(productInputViewModel.Name, productInputViewModel.CostPrice);

            await _productRepository.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            await _productRepository.DeleteAsync(product);

            return NoContent();
        }
    }
}