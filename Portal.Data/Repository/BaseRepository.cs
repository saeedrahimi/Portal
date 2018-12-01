using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.Data;
using Portal.Core.Data.Repository;
using Portal.Core.Entities;

namespace Portal.Data.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {

        private readonly ISqlDbContext DbContext;
        private bool _disposed;

        public BaseRepository(ISqlDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public TEntity GetById(Guid id)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(t => t.Id == id);
        }

        public void Add(TEntity entity)
        {
            DbContext.SetAsAdded(entity);
        }

        public void Update(TEntity entity)
        {
            DbContext.SetAsModified(entity);
        }

        public void Delete(TEntity entity)
        {
            DbContext.SetAsDeleted(entity);
        }

        /*public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                DbContext.Dispose();
            }
            _disposed = true;
        }*/
    }
}
