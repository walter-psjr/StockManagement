using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;

namespace StockManagement.Infrastructure.Repositories
{
    public class StoreRepository : BaseEfRepository<Store>, IStoreRepository
    {
        public StoreRepository(StockManagementContext context) : base(context)
        {
        }
    }
}