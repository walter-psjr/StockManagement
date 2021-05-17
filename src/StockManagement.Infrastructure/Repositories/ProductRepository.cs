using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;

namespace StockManagement.Infrastructure.Repositories
{
    public class ProductRepository : BaseEfRepository<Product>, IProductRepository
    {
        public ProductRepository(StockManagementContext context) : base(context)
        {
        }
    }
}