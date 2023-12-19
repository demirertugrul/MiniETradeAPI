﻿using ETradeAPI.Application.Repositories;
using ETradeAPI.Domain.Entities.Common;
using ETradeAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ETradeAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity //BaseEntity yaptigimiz icin marker pattern'a uygun altyapi sergiledigim icin T türünde ki nesne baseentity'den geliyor class'tan degil. dolayısıyla cagirdigimiz context'teki degerlere ulasabılıyoruz.
    {
        private readonly ETradeAPIDbContext _context;

        public ReadRepository(ETradeAPIDbContext context) // DBContext'i veritabanı değerleri ile eleştirmek için burada ctor ile çağırıyoruz
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>(); /* Burada RepositoryDesign ile kurdugumuz hiyerarşi ile gelen Context Nesnesi 
        burada .Set<T>(); ile veritabanından çağrılıyor. Ve gelen T nesnesi ile methodlarımızla gerekli işlemler yapılıyor. */

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable(); // Tüm read işlemlerini IQueryable ile dönderiyoruz.
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);

        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        //=> await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        //=> await Table.FindAsync(Guid.Parse(id));
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }
    }
}
