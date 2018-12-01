using System.Linq;
using Portal.Dto.Result;

namespace Portal.Core.Service
{
    public interface IBaseService
    {
        Result Commit();
    }
}
