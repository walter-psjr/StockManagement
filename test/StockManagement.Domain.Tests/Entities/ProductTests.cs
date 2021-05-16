using StockManagement.Domain.Entities;
using Xunit;

namespace StockManagement.Domain.Tests.Entities
{
    public class ProductTests
    {
        [Fact]
        public void UpdateShouldUpdatesProperties()
        {
            // Arrange
            var product = new Product("My New Product", 100);

            // Act
            product.Update("My Updated Product", 150);

            // Assert
            Assert.Equal("My Updated Product", product.Name);
            Assert.Equal(150, product.CostPrice);
        }
    }
}