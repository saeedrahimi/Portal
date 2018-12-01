using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.Entities;

namespace Portal.Core.Data
{
    public interface ISqlDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : class;

        void SetAsModified<TEntity>(TEntity entity) where TEntity : class;

        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class;
        void SaveChanges();
        void SaveChangesAsync();

    }
}
