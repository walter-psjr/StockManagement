using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.ViewModels.Input;
using StockManagement.Api.ViewModels.Output;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using StockManagement.Domain.Entities;

namespace StockManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;

        public StoresController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoreOutputViewModel>> GetById(Guid id)
        {
            var store = await _storeRepository.GetByIdAsync(id);

            if (store == null)
                return NotFound();

            var storeOutputViewModel = new StoreOutputViewModel
            {
                Id = store.Id,
                Name = store.Name,
                Address = store.Address
            };

            return Ok(storeOutputViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StoreInputViewModel storeInputViewModel)
        {
            var store = new Store(storeInputViewModel.Name, storeInputViewModel.Address);

            await _storeRepository.SaveAsync(store);

            return CreatedAtAction(nameof(GetById), new {id = store.Id}, null);
        }

        [HttpPut("{id")]
        public async Task<IActionResult> Update(Guid id, StoreInputViewModel storeInputViewModel)
        {
            var store = await _storeRepository.GetByIdAsync(id);

            if (store == null)
                return NotFound();

            store.Update(storeInputViewModel.Name, storeInputViewModel.Address);

            await _storeRepository.UpdateAsync(store);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var store = await _storeRepository.GetByIdAsync(id);

            if (store == null)
                return NotFound();

            await _storeRepository.DeleteAsync(store);

            return NoContent();
        }
    }
}