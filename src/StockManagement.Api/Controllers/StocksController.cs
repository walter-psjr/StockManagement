using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.ViewModels.Input;
using System;
using System.Threading.Tasks;
using StockManagement.Domain.Interfaces.Repositories;

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
            var product = await _productRepository.GetByIdAsync(productId);

            store.CreateStock(product, stockInputViewModel.Amount);

            await _storeRepository.UpdateAsync(store);

            return Created(string.Empty, null);
        }

        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock/increase")]
        public async Task<IActionResult> Increase(Guid storeId, Guid productId, StockInputViewModel stockInputViewModel)
        {
            var store = await _storeRepository.GetByIdWithStockItemsAsync(storeId);
            var product = await _productRepository.GetByIdAsync(productId);

            store.IncreaseStock(product, stockInputViewModel.Amount);

            await _storeRepository.UpdateAsync(store);

            return Accepted();
        }

        [HttpPost("~/api/stores/{storeId}/products/{productId}/stock/decrease")]
        public async Task<IActionResult> Decrease(Guid storeId, Guid productId, StockInputViewModel stockInputViewModel)
        {
            var store = await _storeRepository.GetByIdWithStockItemsAsync(storeId);
            var product = await _productRepository.GetByIdAsync(productId);

            store.DecreaseStock(product, stockInputViewModel.Amount);

            await _storeRepository.UpdateAsync(store);

            return Accepted();
        }
    }
}