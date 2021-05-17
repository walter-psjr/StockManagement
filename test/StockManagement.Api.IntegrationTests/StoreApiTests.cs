using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StockManagement.Domain.Entities;
using StockManagement.Infrastructure;
using Xunit;

namespace StockManagement.Api.IntegrationTests
{
    public class StoreApiTests
    {
        private readonly HttpClient _client;
        private Store _store;

        public StoreApiTests()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<StockManagementContext>));
                    services.AddDbContext<StockManagementContext>(options =>
                    {
                        options.UseInMemoryDatabase("StockManagement");
                    });
                });

            var server = new TestServer(webHostBuilder);

            SeedDatabase(server);

            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetByIdReturnsStoreGivenExistentId()
        {
            // Arrange
            var storeId = _store.Id;

            // Act
            var result = await _client.GetFromJsonAsync<Store>($"/api/stores/{storeId}");

            // Assert
            Assert.Equal(_store.Id, result.Id);
            Assert.Equal(_store.Name, result.Name);
            Assert.Equal(_store.Address, result.Address);
        }

        private void SeedDatabase(TestServer server)
        {
            var context = server.Services.GetRequiredService<StockManagementContext>();
            var store = new Store("My Store", "My Store Address");
            context.Stores.Add(store);
            context.SaveChanges();

            _store = store;
        }
    }
}