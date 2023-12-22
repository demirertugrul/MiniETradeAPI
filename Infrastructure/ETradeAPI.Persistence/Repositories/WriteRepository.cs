using ETradeAPI.Application.Repositories;
using ETradeAPI.Domain.Entities.Common;
using ETradeAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ETradeAPI.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly ETradeAPIDbContext _context;

        public WriteRepository(ETradeAPIDbContext context)
        {
            this._context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            // EntityEntry entityEntry = _context.Entry(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }

        public bool Remove(T model) 
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }
        public async Task<bool> RemoveAsync(string id)
        {
            var model = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            return Remove(model);
        }

        public bool Update(T model)
        {
            EntityEntry entityEntry = Table.Update(model); /* tracking edilmese bile(context üzerinden gelmese bile) ef'nin updatei
            çalışıyor sonradan bakılacak. track etmesen bile update et diyorsan context.Update çalışıyor. */
            return entityEntry.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync() // YAPILAN CRUD İŞLEMLERİNİN UYGULANMASI İÇİN KENDİMİZE GÖRE YAZDIK.
         => await _context.SaveChangesAsync();
    }
}
