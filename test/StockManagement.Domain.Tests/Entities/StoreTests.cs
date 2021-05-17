using System;
using StockManagement.Domain.Entities;
using Xunit;

namespace StockManagement.Domain.Tests.Entities
{
    public class StoreTests
    {
        [Fact]
        public void CreateStockCreatesStockGivenAProductAndAmount()
        {
            // Arrange
            var product = new Product("My Product", 100)
            {
                Id = Guid.NewGuid()
            };
            var amount = 10;
            var store = new Store("My Store", "My Store Address");

            // Act
            store.CreateStock(product, amount);

            // Assert
            Assert.Equal(store.GetStockAmount(product), amount);
        }

        [Fact]
        public void CreateStockThrowsExceptionGivenDuplicatedProduct()
        {
            // Arrange
            var product = new Product("My Product", 100)
            {
                Id = Guid.NewGuid()
            };
            var product2 = new Product("My Product", 150)
            {
                Id = product.Id
            };
            var amount = 10;
            var store = new Store("My Store", "My Store Address");

            // Act
            store.CreateStock(product, amount);

            // Assert
            Assert.Throws<InvalidOperationException>(() => store.CreateStock(product2, amount));
        }

        [Fact]
        public void IncreaseStockUpdatesAmountGivenAValidProduct()
        {
            // Arrange
            var product = new Product("My Product", 100)
            {
                Id = Guid.NewGuid()
            };
            var amount = 10;
            var store = new Store("My Store", "My Store Address");

            // Act
            store.IncreaseStock(product, amount);

            // Assert
        }
    }
}