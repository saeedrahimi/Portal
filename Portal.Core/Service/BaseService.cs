using System;
using System.Linq;
using Portal.Core.Data;
using Portal.Core.Entities;
using Portal.Dto.Result;

namespace Portal.Core.Service
{
    public class BaseService : IBaseService
    {
        private readonly ISqlDbContext _dbContext;

        public BaseService(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       public Result Commit()
        {
            try
            {
                _dbContext.SaveChanges();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }
           
        }
    }


    
}
