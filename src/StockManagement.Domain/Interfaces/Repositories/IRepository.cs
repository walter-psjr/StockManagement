using System;
using System.Threading.Tasks;

namespace StockManagement.Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(Guid id);

        Task<T> SaveAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}