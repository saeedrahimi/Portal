using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Portal.Core.Entities.Company;

namespace Portal.Data.EntityConfigurations.Bulletin
{
    internal class BulletinConfig : EntityTypeConfiguration<Core.Entities.Bulletin.Bulletin>
    {
        public BulletinConfig()
        {
            ToTable(@"tbl_Bulletin").HasKey(x => x.Id);
            Property(p => p.Title).IsRequired().HasMaxLength(100);
            Property(p => p.Description).HasMaxLength(1000);
            Property(p => p.Issuer).IsRequired();
        }
    }
}
