using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Interfaces.Repositories;

namespace StockManagement.Infrastructure.Repositories
{
    public class StoreRepository : BaseEfRepository<Store>, IStoreRepository
    {
        private readonly StockManagementContext _context;

        public StoreRepository(StockManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Store> GetByIdWithStockItemsAsync(Guid storeId)
        {
            return await _context.Set<Store>()
                .Include(x => x.StockItems)
                .FirstOrDefaultAsync(x => x.Id == storeId);
        }
    }
}