using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.Entities.Company;

namespace Portal.Data.EntityConfigurations.Company
{
    internal class CompanyGroupConfig : EntityTypeConfiguration<CompanyGroup>
    {
        public CompanyGroupConfig()
        {
            ToTable(@"tbl_CompanyGroup").HasKey(x => x.Id);
            Property(p => p.Title).IsRequired().HasMaxLength(100)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Title"){IsUnique = true}));
            Property(p => p.Description).HasMaxLength(1000);
            HasMany(r => r.BulletinTypes).WithMany();
        }
    }
}
