using System;
using System.Threading.Tasks;
using StockManagement.Domain.Entities;

namespace StockManagement.Domain.Interfaces.Repositories
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<Store> GetByIdWithStockItemsAsync(Guid storeId);
    }
}