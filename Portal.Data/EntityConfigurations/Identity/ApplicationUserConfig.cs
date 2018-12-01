using System.Data.Entity.ModelConfiguration;
using Portal.Data.Identity.Models;

namespace Portal.Data.EntityConfigurations.Identity
{
    internal class ApplicationUserConfig : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfig()
        {
            ToTable("Users").HasKey(r => r.Id);
            HasRequired(r => r.CompanyGroup);
            HasMany(r => r.Bulletins).WithRequired().HasForeignKey(r=>r.Issuer);
        }
    }
}
