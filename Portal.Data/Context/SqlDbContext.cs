using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Core.Data;
using Portal.Core.Entities;
using Portal.Data.Identity.Models;
using System.Data.Entity.Infrastructure;
using Portal.Core.Entities.Bulletin;
using Portal.Core.Entities.Company;
using Portal.Data.EntityConfigurations.Bulletin;
using Portal.Data.EntityConfigurations.BulletinType;
using Portal.Data.EntityConfigurations.Company;
using Portal.Data.EntityConfigurations.Identity;
using Portal.Data.Migrations;

namespace Portal.Data.Context
{
    public class SqlDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim> , ISqlDbContext
    {
        public SqlDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<CompanyGroup> CompanyGroups { get; set; }
        public DbSet<Bulletin> Bulletins { get; set; }
        public DbSet<BulletinType> BulletinTypes { get; set; }
        public DbSet<BulletinTransfer> BulletinTransfers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CompanyGroupConfig());
            modelBuilder.Configurations.Add(new BulletinConfig());
            modelBuilder.Configurations.Add(new BulletinTypeConfig());
            modelBuilder.Configurations.Add(new BulletinTransferConfig());

            modelBuilder.Configurations.Add(new ApplicationUserConfig());


            modelBuilder.Entity<CustomRole>().ToTable("Roles").HasKey(r=>r.Id);
            modelBuilder.Entity<CustomUserClaim>().ToTable("UserClaims").HasKey(r=>r.Id);
            modelBuilder.Entity<CustomUserRole>().ToTable("UserRoles").HasKey(r=>r.RoleId);
            modelBuilder.Entity<CustomUserLogin>().ToTable("UserLogins").HasKey(r=>r.ProviderKey);
        }

        #region IBaseDbContextcs
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Added;
            //UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }

        public void SaveChanges()
        {
            base.SaveChanges();
        }
        public void SaveChangesAsync()
        {
            base.SaveChangesAsync();
        }

        #endregion
        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }

        private DbEntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : class
        {
            var dbEntityEntry = Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }



    }
}
