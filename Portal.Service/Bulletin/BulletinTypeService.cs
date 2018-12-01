using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.Core.Data;
using Portal.Core.Data.Repository;
using Portal.Core.Entities.Bulletin;
using Portal.Core.Entities.Company;
using Portal.Core.Service;
using Portal.Core.Service.Bulletin;
using Portal.Dto.Dtos.Bulletin;
using Portal.Dto.Dtos.Company;
using Portal.Dto.Result;

namespace Portal.Service.Bulletin
{
    public class BulletinTypeService :BaseService, IBulletinTypeService
    {
        private readonly IBulletinTypeRepository _bulletinTypeRepository;
        private readonly ISqlDbContext dbContext;

        public BulletinTypeService(IBulletinTypeRepository typeRepository, ISqlDbContext dbContext):base(dbContext)
        {
            _bulletinTypeRepository = typeRepository;
            this.dbContext = dbContext;
        }
        public Result AddType(BulletinTypeDto item)
        {
            try
            {
                var group = new BulletinType()
                {
                    Id = Guid.NewGuid(),
                    Title = item.Title,
                    Description = item.Description
                };
                _bulletinTypeRepository.Add(group);
                return base.Commit();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }

        }
        public Result EditType(BulletinTypeDto item)
        {
            try
            {
                var group = _bulletinTypeRepository.GetById(item.Id);

                if (group == null)
                    return Result.Failed("Group not found");
                group.Title = item.Title;
                group.Description = item.Description;

                _bulletinTypeRepository.Update(group);
                return base.Commit();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }

        }

        public async Task<Result<List<BulletinTypeDto>>> GetAllGroupsAsync()
        {
            try
            {
                var groups = await _bulletinTypeRepository.GetAllAsync();
                var castedGroups = groups.Select(s => new BulletinTypeDto()
                {
                    Id=s.Id,
                    Title = s.Title,
                    Description = s.Description
                }).ToList();
                return Result.Success(castedGroups);
            }
            catch (Exception ex)
            {
                return Result.Failed<List<BulletinTypeDto>> (ex.Message);
            }
        }

        public Result<BulletinTypeDto> GetById(Guid id)
        {
            try
            {
                var group = _bulletinTypeRepository.GetById(id);
                var castedGroup = new BulletinTypeDto()
                {
                    Id = group.Id,
                    Title = group.Title,
                    Description = group.Description
                };
                return Result.Success(castedGroup);
            }
            catch (Exception ex)
            {
                return Result.Failed<BulletinTypeDto>(ex.Message);
            }
        }


        public Result Delete(BulletinTypeDto bulletinType)
        {
            try
            {
                var group = _bulletinTypeRepository.GetById(bulletinType.Id);

                if (group == null)
                    return Result.Failed("Group not found");
                _bulletinTypeRepository.Delete(group);
                return base.Commit();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }
        }
    }
}
