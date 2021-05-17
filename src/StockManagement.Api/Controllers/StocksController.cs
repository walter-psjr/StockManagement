using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.ViewModels.Input;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace StockManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class StocksController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;

        public StocksController(IStoreRepository storeRepository, IProductRepository productRepository)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Creates stock for the specified Store and Product
        /// </summary>
        /// <param name="storeId">The Store Id</param>
        /// <param name="productId">The Product Id</param>
        /// <param name="stockInputViewModel">Stock info</param>
        /// <returns></returns>
        /// <response code="201">Returns a location header for the new store</response>
        /// <response code="404">Store and/or Product not found</response>
        /// <response code="400">Input data validation failed</response>
        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Guid storeId, Guid productId, StockInputViewModel stockInputViewModel)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);

            if (store == null)
                return NotFound($"Store {storeId} not found");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                return NotFound($"Product {productId} not found");

            store.CreateStock(product, stockInputViewModel.Amount);

            await _storeRepository.UpdateAsync(store);

            return Created(string.Empty, null);
        }

        /// <summary>
        /// Add item to stock of a Store for the specific product
        /// </summary>
        /// <param name="storeId">The Store Id</param>
        /// <param name="productId">The Product Id</param>
        /// <param name="stockInputViewModel">Stock Info</param>
        /// <returns></returns>
        /// <response code="202"></response>
        /// <response code="404">Store and/or Product not found</response>
        /// <response code="400">Input data validation failed</response>
        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock/increase")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Increase(Guid storeId, Guid productId, StockInputViewModel stockInputViewModel)
        {
            var store = await _storeRepository.GetByIdWithStockItemsAsync(storeId);

            if (store == null)
                return NotFound($"Store {storeId} not found");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                return NotFound($"Product {productId} not found");

            store.IncreaseStock(product, stockInputViewModel.Amount);

            await _storeRepository.UpdateAsync(store);

            return Accepted();
        }

        /// <summary>
        /// Removes items from Store stock for a specific product
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="productId"></param>
        /// <param name="stockInputViewModel"></param>
        /// <returns></returns>
        /// <response code="202"></response>
        /// <response code="404">Store and/or Product not found</response>
        /// <response code="400">Input data validation failed</response>
        /// <response code="409">Some business rule not pass</response>
        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock/decrease")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Decrease(Guid storeId, Guid productId, StockInputViewModel stockInputViewModel)
        {
            var store = await _storeRepository.GetByIdWithStockItemsAsync(storeId);

            if (store == null)
                return NotFound($"Store {storeId} not found");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                return NotFound($"Product {productId} not found");

            try
            {
                store.DecreaseStock(product, stockInputViewModel.Amount);

                await _storeRepository.UpdateAsync(store);

                return Accepted();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}