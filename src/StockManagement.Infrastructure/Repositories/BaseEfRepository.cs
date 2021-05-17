using Microsoft.EntityFrameworkCore;
using StockManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Repositories
{
    public class BaseEfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly StockManagementContext _context;

        public BaseEfRepository(StockManagementContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> SaveAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}