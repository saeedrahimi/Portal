using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.Data;
using Portal.Core.Data.Repository;
using Portal.Core.Entities.Company;

namespace Portal.Data.Repository.Company
{
    public class CompanyGroupRepository : BaseRepository<CompanyGroup> , ICompanyGroupRepository
    {
        public CompanyGroupRepository(ISqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}
