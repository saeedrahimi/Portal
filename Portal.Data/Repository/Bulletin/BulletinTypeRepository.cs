using Portal.Core.Data;
using Portal.Core.Data.Repository;
using Portal.Core.Entities.Bulletin;
using Portal.Core.Entities.Company;

namespace Portal.Data.Repository.Bulletin
{
    public class BulletinTypeRepository : BaseRepository<BulletinType> , IBulletinTypeRepository
    {
        public BulletinTypeRepository(ISqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}
