using Microsoft.AspNetCore.Mvc;
using Moq;
using StockManagement.Api.Controllers;
using StockManagement.Api.ViewModels.Output;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using StockManagement.Api.ViewModels.Input;
using Xunit;

namespace StockManager.Api.UnitTests.Controllers
{
    public class StoresControllerTests
    {
        [Fact]
        public async Task GetByIdReturnsStoreGivenExistentId()
        {
            // Arrange
            var storeId = Guid.NewGuid();
            var store = new Store("My Store", "One Infinite Loop, Cupertino, CA 95014, US")
            {
                Id = storeId
            };

            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync(store);

            var storesController = new StoresController(storeRepositoryMock.Object);

            // Act
            var response = await storesController.GetById(storeId);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response.Result);
            var storeOutputViewModel = Assert.IsType<StoreOutputViewModel>(result.Value);
            Assert.Equal(store.Id, storeOutputViewModel.Id);
            Assert.Equal(store.Name, storeOutputViewModel.Name);
            Assert.Equal(store.Address, storeOutputViewModel.Address);
        }

        [Fact]
        public async Task GetByIdReturnsNotFoundGivenNonexistentId()
        {
            // Arrange
            var storeId = Guid.NewGuid();
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync((Store)null);

            var storesController = new StoresController(storeRepositoryMock.Object);

            // Act
            var response = await storesController.GetById(storeId);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task CreateReturnsCreatedAtGivenAValidNewStore()
        {
            // Arrange
            var storeInputViewModel = new StoreInputViewModel
            {
                Name = "My New Store",
                Address = "The New Address"
            };
            var newStore = new Store(storeInputViewModel.Name, storeInputViewModel.Address)
            {
                Id = Guid.NewGuid()
            };
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.SaveAsync(newStore)).ReturnsAsync(newStore);

            var storesController = new StoresController(storeRepositoryMock.Object);

            // Act
            var response = await storesController.Create(storeInputViewModel);

            // Assert
            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact]
        public async Task UpdateReturnsNoContentGivenValidStore()
        {
            // Arrange
            var storeId = Guid.NewGuid();
            var storeInputViewModel = new StoreInputViewModel
            {
                Name = "Updated My Store",
                Address = "Updated My Store Address"
            };
            var store = new Store("My Store", "My Store Address")
            {
                Id = storeId
            };
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync(store);
            storeRepositoryMock.Setup(m => m.UpdateAsync(store)).ReturnsAsync(store);

            var storesController = new StoresController(storeRepositoryMock.Object);

            // Act
            var response = await storesController.Update(storeId, storeInputViewModel);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task UpdateReturnsNotFoundGivenNonexistentId()
        {
            // Arrange
            var storeId = Guid.NewGuid();
            var storeInputViewModel = new StoreInputViewModel
            {
                Name = "Updated My Store",
                Address = "Updated My Store Address"
            };
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync((Store)null);

            var storesController = new StoresController(storeRepositoryMock.Object);

            // Act
            var response = await storesController.Update(storeId, storeInputViewModel);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeleteReturnsNotFoundGivenNonexistentId()
        {
            // Arrange
            var storeId = Guid.NewGuid();
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync((Store)null);

            var storesController = new StoresController(storeRepositoryMock.Object);

            // Act
            var response = await storesController.Delete(storeId);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeleteReturnsNoContentGivenExistentId()
        {
            // Arrange
            var storeId = Guid.NewGuid();
            var store = new Store("My Store", "My Store Address")
            {
                Id = storeId
            };
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync(store);

            var storesController = new StoresController(storeRepositoryMock.Object);

            // Act
            var response = await storesController.Delete(storeId);

            // Assert
            storeRepositoryMock.Verify(m => m.DeleteAsync(store), Times.Once);
            Assert.IsType<NoContentResult>(response);
        }
    }
}