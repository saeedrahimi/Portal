using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Portal.Core.Entities.Company;

namespace Portal.Data.EntityConfigurations.BulletinType
{
    internal class BulletinTransferConfig : EntityTypeConfiguration<Core.Entities.Bulletin.BulletinTransfer>
    {
        public BulletinTransferConfig()
        {
            ToTable(@"tbl_BulletinTransfers").HasKey(x => x.Id);
            Property(p => p.Description).HasMaxLength(1000);
            HasRequired(r => r.Bulletin).WithOptional();
        }
    }
}
