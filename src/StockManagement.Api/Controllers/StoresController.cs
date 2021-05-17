using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.ViewModels.Input;
using StockManagement.Api.ViewModels.Output;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StockManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;

        public StoresController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        /// <summary>
        /// Gets a Store by Id
        /// </summary>
        /// <param name="id">The Store Id</param>
        /// <returns>The Store</returns>
        /// <response code="200">Returns the found store</response>
        /// <response code="404">If the store is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a new Store
        /// </summary>
        /// <param name="storeInputViewModel">The new store data</param>
        /// <returns></returns>
        /// <response code="201">Returns a location header for the new store</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(StoreInputViewModel storeInputViewModel)
        {
            var store = new Store(storeInputViewModel.Name, storeInputViewModel.Address);

            await _storeRepository.SaveAsync(store);

            return CreatedAtAction(nameof(GetById), new { id = store.Id }, null);
        }

        /// <summary>
        /// Updates a Store
        /// </summary>
        /// <param name="id">The Store Id</param>
        /// <param name="storeInputViewModel">The Store data to be updated</param>
        /// <returns></returns>
        /// <response code="204"></response>
        /// <response code="404">Store not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, StoreInputViewModel storeInputViewModel)
        {
            var store = await _storeRepository.GetByIdAsync(id);

            if (store == null)
                return NotFound();

            store.Update(storeInputViewModel.Name, storeInputViewModel.Address);

            await _storeRepository.UpdateAsync(store);

            return NoContent();
        }

        /// <summary>
        /// Deletes a Store
        /// </summary>
        /// <param name="id">The Store Id</param>
        /// <returns></returns>
        /// <response code="204"></response>
        /// <response code="404">Store not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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