using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Core.Data;
using Portal.Data.Identity.Contracts;
using Portal.Data.Identity.Models;

namespace Portal.Service.Identity
{
    public class CustomRoleStore :
        RoleStore<CustomRole, int, CustomUserRole>,
        ICustomRoleStore
    {
        private readonly ISqlDbContext _context;

        public CustomRoleStore(ISqlDbContext context)
            : base((DbContext)context)
        {
            _context = context;
        }
    }
}