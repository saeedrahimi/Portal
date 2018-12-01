using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Portal.Core.Entities.Bulletin;
using Portal.Core.Entities.Company;

namespace Portal.Data.Migrations
{


    public sealed class Configuration : DbMigrationsConfiguration<Portal.Data.Context.SqlDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Portal.Data.Context.SqlDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            #region CompanyGroup

            var companyGroupsList = new List<string> { "تولیدی", "سرمایه گذاری", "بانک" };
            foreach (var groupTitle in companyGroupsList)
            {
                if (context.CompanyGroups.Any(a => a.Title == groupTitle))
                    continue;
                context.CompanyGroups.Add(new CompanyGroup()
                {
                    Id = Guid.NewGuid(),
                    Title = groupTitle
                });
            }
            #endregion
            #region BulletinType

            var bulletinTypeList = new List<string> { "ترازنامه", "صورت سود و زیان", "آگهی مجمع" , "تصمیمات" };
            foreach (var typeTitle in bulletinTypeList)
            {
                if (context.BulletinTypes.Any(a => a.Title == typeTitle))
                    continue;
                context.BulletinTypes.Add(new BulletinType()
                {
                    Id = Guid.NewGuid(),
                    Title = typeTitle
                });
            }
            #endregion
        }
    }
}
