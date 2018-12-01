using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.Data.Repository;
using Portal.Core.Entities.Company;
using Portal.Dto.Dtos.Company;
using Portal.Dto.Result;

namespace Portal.Core.Service.Company
{
    public interface ICompanyGroupService : IBaseService
    {
        Result AddGroup(CompanyGroupDto item);
        Result EditGroup(CompanyGroupEditDto item, Guid[] selectedTypes);
        Task<Result<List<CompanyGroup>>> GetAllGroupsAsync();
        Result<CompanyGroup> GetById(Guid id);
        Result Delete(CompanyGroup companyGroup);

    }
}
