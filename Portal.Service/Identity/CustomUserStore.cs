using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Core.Data;
using Portal.Data.Context;
using Portal.Data.Identity.Contracts;
using Portal.Data.Identity.Models;

namespace Portal.Service.Identity
{
    public class CustomUserStore :
        UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>,
        ICustomUserStore
    {
        private readonly ISqlDbContext _context;

        public CustomUserStore(ISqlDbContext context)
            : base((SqlDbContext)context)
        {
            _context = context;
        }

    }
}