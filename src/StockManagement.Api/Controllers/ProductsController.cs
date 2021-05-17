using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Produces("application/json")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Gets a Product By Id
        /// </summary>
        /// <param name="id">The Product Id</param>
        /// <returns>Returns the found Product</returns>
        /// <response code="200">Returns the found product</response>
        /// <response code="404">If the product is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a new Product
        /// </summary>
        /// <param name="productInputViewModel">The new product data</param>
        /// <returns></returns>
        /// <response code="201">Returns a location header for the new product</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(ProductInputViewModel productInputViewModel)
        {
            var product = new Product(productInputViewModel.Name, productInputViewModel.CostPrice);

            await _productRepository.SaveAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, null);
        }

        /// <summary>
        /// Updates a Product
        /// </summary>
        /// <param name="id">The Product Id</param>
        /// <param name="productInputViewModel">The Store data to be updated</param>
        /// <returns></returns>
        /// <response code="204"></response>
        /// <response code="404">Product not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, ProductInputViewModel productInputViewModel)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            product.Update(productInputViewModel.Name, productInputViewModel.CostPrice);

            await _productRepository.UpdateAsync(product);

            return NoContent();
        }

        /// <summary>
        /// Deletes a Product
        /// </summary>
        /// <param name="id">The Product Id</param>
        /// <returns></returns>
        /// <response code="204"></response>
        /// <response code="404">Product not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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