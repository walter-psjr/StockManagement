using Microsoft.AspNetCore.Mvc;
using Moq;
using StockManagement.Api.Controllers;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using StockManagement.Api.ViewModels.Input;
using StockManagement.Api.ViewModels.Output;
using Xunit;

namespace StockManager.Api.UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetByIdReturnsProductGivenExistentId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("My Product", 100)
            {
                Id = productId
            };
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync(product);

            var productsController = new ProductsController(productRepositoryMock.Object);

            // Act
            var response = await productsController.GetById(productId);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response.Result);
            var productOutputViewModel = Assert.IsType<ProductOutputViewModel>(result.Value);
            Assert.Equal(product.Id, productOutputViewModel.Id);
            Assert.Equal(product.Name, productOutputViewModel.Name);
            Assert.Equal(product.CostPrice, productOutputViewModel.CostPrice);
        }

        [Fact]
        public async Task GetByIdReturnsNotFoundGivenNonexistentId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            var productsController = new ProductsController(productRepositoryMock.Object);

            // Act
            var response = await productsController.GetById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task CreateReturnsCreatedAtGivenAValidNewProduct()
        {
            // Arrange
            var productInputViewModel = new ProductInputViewModel
            {
                Name = "My New Product",
                CostPrice = 150
            };
            var newProduct = new Product(productInputViewModel.Name, productInputViewModel.CostPrice)
            {
                Id = Guid.NewGuid()
            };
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.SaveAsync(newProduct)).ReturnsAsync(newProduct);

            var productsController = new ProductsController(productRepositoryMock.Object);

            // Act
            var response = await productsController.Create(productInputViewModel);

            // Assert
            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact]
        public async Task UpdateReturnsNoContentGivenAValidProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productInputViewModel = new ProductInputViewModel
            {
                Name = "Updated My New Product",
                CostPrice = 200
            };
            var product = new Product("My Product", 150)
            {
                Id = productId
            };
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync(product);
            productRepositoryMock.Setup(m => m.UpdateAsync(product)).ReturnsAsync(product);

            var productsController = new ProductsController(productRepositoryMock.Object);

            // Act
            var response = await productsController.Update(productId, productInputViewModel);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task UpdateReturnsNotFoundGivenNonexistentId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productInputViewModel = new ProductInputViewModel
            {
                Name = "Updated My New Product",
                CostPrice = 200
            };
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            var productsController = new ProductsController(productRepositoryMock.Object);

            // Act
            var response = await productsController.Update(productId, productInputViewModel);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeleteReturnsNotFoundGivenANonexistentId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            var productsController = new ProductsController(productRepositoryMock.Object);

            // Act
            var response = await productsController.Delete(productId);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeleteReturnsNoContentGivenExistentId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("My Product", 200);
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.GetByIdAsync(productId)).ReturnsAsync(product);

            var productsController = new ProductsController(productRepositoryMock.Object);

            // Act
            var response = await productsController.Delete(productId);

            // Assert
            productRepositoryMock.Verify(m => m.DeleteAsync(product), Times.Once);
            Assert.IsType<NoContentResult>(response);
        }
    }
}