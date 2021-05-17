using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.Controllers;
using StockManagement.Api.ViewModels.Input;
using System;
using System.Threading.Tasks;
using Moq;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;
using Xunit;

namespace StockManager.Api.UnitTests.Controllers
{
    public class StocksControllerTests
    {
        [Fact]
        public async Task CreateReturnsCreatedAtActionGivenValidStock()
        {
            // Arrange
            var storeId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var store = new Store("My Store", "My Store Address")
            {
                Id = storeId
            };
            var product = new Product("My Product", 150)
            {
                Id = productId
            };
            var stockInputViewModel = new StockInputViewModel
            {
                Amount = 10
            };
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync(store);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync(product);

            var stocksController = new StocksController(storeRepositoryMock.Object, productRepositoryMock.Object);

            // Act
            var response = await stocksController.Create(storeId, productId, stockInputViewModel);

            // Assert
            Assert.IsType<CreatedResult>(response);
        }

        [Fact]
        public async Task IncreaseReturnsAcceptedGivenAValidProductAndAmount()
        {
            var storeId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var store = new Store("My Store", "My Store Address")
            {
                Id = storeId
            };
            var product = new Product("My Product", 150)
            {
                Id = productId
            };
            var stockInputViewModel = new StockInputViewModel
            {
                Amount = 10
            };
            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock.Setup(m => m.GetByIdAsync(storeId)).ReturnsAsync(store);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync(product);

            store.CreateStock(product, 10);

            var stocksController = new StocksController(storeRepositoryMock.Object, productRepositoryMock.Object);

            // Act
            var response = await stocksController.Increase(storeId, productId, stockInputViewModel);

            // Assert
            Assert.IsType<AcceptedResult>(response);
        }
    }
}