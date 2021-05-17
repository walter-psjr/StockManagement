using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.ViewModels.Input;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace StockManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;

        public StocksController(IStoreRepository storeRepository, IProductRepository productRepository)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
        }

        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock")]
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

        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock/increase")]
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

        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock/decrease")]
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