using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Core.Data;
using Portal.Core.Data.Repository;
using Portal.Core.Entities.Company;
using Portal.Core.Service;
using Portal.Core.Service.Company;
using Portal.Data.Repository.Company;
using Portal.Dto.Dtos.Company;
using Portal.Dto.Result;

namespace Portal.Service.Company
{
    public class CompanyGroupService :BaseService, ICompanyGroupService
    {
        private readonly ICompanyGroupRepository _groupRepository;
        private readonly IBulletinTypeRepository _bulletinTypeRepository;
        private readonly ISqlDbContext dbContext;

        public CompanyGroupService( ICompanyGroupRepository groupRepository, ISqlDbContext dbContext, IBulletinTypeRepository bulletinTypeRepository):base(dbContext)
        {
            _groupRepository = groupRepository;
            this.dbContext = dbContext;
            _bulletinTypeRepository = bulletinTypeRepository;
        }
        public Result AddGroup(CompanyGroupDto item)
        {
            try
            {
                var group = new CompanyGroup()
                {
                    Id = Guid.NewGuid(),
                    Title = item.Title,
                    Description = item.Description
                };
                _groupRepository.Add(group);
                return base.Commit();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }

        }
        public Result EditGroup(CompanyGroupEditDto item,Guid[] selectedTypes)
        {
            try
            {
                var group = _groupRepository.GetById(item.Id);
                if (group == null)
                    return Result.Failed("Group not found");

                group.BulletinTypes.Clear();
                foreach (var selectedType in selectedTypes)
                {
                    group.BulletinTypes.Add(_bulletinTypeRepository.GetById(selectedType));
                }
                group.Title = item.Title;
                group.Description = item.Description;

                _groupRepository.Update(group);
                return base.Commit();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }

        }

        public async Task<Result<List<CompanyGroup>>> GetAllGroupsAsync()
        {
            try
            {
                var groups = await _groupRepository.GetAllAsync();
                return Result.Success(groups);
            }
            catch (Exception ex)
            {
                return Result.Failed<List<CompanyGroup>> (ex.Message);
            }
        }

        public Result<CompanyGroup> GetById(Guid id)
        {
            try
            {
                var group = _groupRepository.GetById(id);
                return Result.Success(group);
            }
            catch (Exception ex)
            {
                return Result.Failed<CompanyGroup>(ex.Message);
            }
        }


        public Result Delete(CompanyGroup companyGroup)
        {
            try
            {
               _groupRepository.Delete(companyGroup);
                return base.Commit();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }
        }
    }
}
