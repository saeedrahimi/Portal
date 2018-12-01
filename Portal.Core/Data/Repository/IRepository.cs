using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Core.Entities;

namespace Portal.Core.Data.Repository
{
    public interface IRepository<TEntity>  where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(Guid id);
        TEntity GetById(Guid id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
