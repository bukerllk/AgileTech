using AgileTech_API.Data;
using AgileTech_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AgileTech_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicatioDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicatioDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>>? filter = null, bool traked = true)
        {
            IQueryable<T> query = dbSet;

            if (!traked) 
            {
                query = query.AsNoTracking();
            }
            
            if (filter != null)
            { 
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
