using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Portal.Core.Entities.Company;

namespace Portal.Data.EntityConfigurations.BulletinType
{
    internal class BulletinTypeConfig : EntityTypeConfiguration<Core.Entities.Bulletin.BulletinType>
    {
        public BulletinTypeConfig()
        {
            ToTable(@"tbl_BulletinTypes").HasKey(x => x.Id);
            Property(p => p.Title).IsRequired().HasMaxLength(100);
            Property(p => p.Description).HasMaxLength(1000);
        }
    }
}
