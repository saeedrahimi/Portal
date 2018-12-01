using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Core.Entities.Company;
using Portal.Dto.Dtos.Bulletin;
using Portal.Dto.Dtos.Company;
using Portal.Dto.Result;

namespace Portal.Core.Service.Bulletin
{
    public interface IBulletinTypeService : IBaseService
    {
        Result AddType(BulletinTypeDto item);
        Result EditType(BulletinTypeDto item);
        Task<Result<List<BulletinTypeDto>>> GetAllGroupsAsync();
        Result<BulletinTypeDto> GetById(Guid id);
        Result Delete(BulletinTypeDto companyGroup);

    }
}
