using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Portal.Core.Data;
using Portal.Core.Data.Repository;
using Portal.Data.Identity.Contracts;
using Portal.Data.Identity.Models;

namespace Portal.Service.Identity
{
    public class ApplicationUserManager
        : UserManager<ApplicationUser, int>,
        IApplicationUserManager
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IApplicationRoleManager _roleManager;
        private readonly ICustomUserStore _store;
        private readonly ISqlDbContext _dbContext;
        private readonly ICompanyGroupRepository _companyGroupRepository;
        private readonly IDbSet<ApplicationUser> _users;
        private readonly Lazy<Func<IIdentity>> _identity;
        private ApplicationUser _user;

        public ApplicationUserManager(
            ICustomUserStore store,
            ISqlDbContext dbContext,
            Lazy<Func<IIdentity>> identity, // For lazy loading -> Controller gets constructed before the HttpContext has been set by ASP.NET.
            IApplicationRoleManager roleManager,
            IDataProtectionProvider dataProtectionProvider, ICompanyGroupRepository companyGroupRepository)
            : base((IUserStore<ApplicationUser, int>)store)
        {
            _store = store;
            _dbContext = dbContext;
            _identity = identity;
            _users = _dbContext.Set<ApplicationUser>();
            _roleManager = roleManager;
            _dataProtectionProvider = dataProtectionProvider;
            _companyGroupRepository = companyGroupRepository;

            createApplicationUserManager();
        }

        public ApplicationUser FindById(int userId)
        {
            return _users.Find(userId);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser applicationUser)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie).ConfigureAwait(false);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("user-email", applicationUser.Email));

            return userIdentity;
        }

        public Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return this.Users.ToListAsync();
        }

        public ApplicationUser GetCurrentUser()
        {
            return _user ?? (_user = this.FindById(GetCurrentUserId()));
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _user ?? (_user = await this.FindByIdAsync(GetCurrentUserId()).ConfigureAwait(false));
        }

        public int GetCurrentUserId()
        {
            return _identity.Value().GetUserId<int>();
        }

        public async Task<bool> HasPassword(int userId)
        {
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            return user != null && user.PasswordHash != null;
        }

        public async Task<bool> HasPhoneNumber(int userId)
        {
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            return user != null && user.PhoneNumber != null;
        }

        public IdentityResult AddToCompanyGroup(int userId, Guid groupId)
        {
            var user = FindById(userId);
            if(user == null)
                return IdentityResult.Failed("Not Found");
            var companyGroup = _companyGroupRepository.GetById(groupId);
            if(companyGroup == null)
                return IdentityResult.Failed("Not Found");
            user.CompanyGroup = companyGroup;
            _dbContext.SetAsModified(user);
            _dbContext.SaveChanges();
            return IdentityResult.Success;

        }

        public Func<CookieValidateIdentityContext, Task> OnValidateIdentity()
        {
            return SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                         validateInterval: TimeSpan.FromSeconds(0),
                         regenerateIdentityCallback: (manager, user) => manager.GenerateUserIdentityAsync(user),
                         getUserIdCallback: claimsIdentity => claimsIdentity.GetUserId<int>());
        }

        public void SeedDatabase()
        {
            const string name = "admin@example.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = _roleManager.FindRoleByName(roleName);
            if (role == null)
            {
                role = new CustomRole(roleName);
                var roleResult = _roleManager.CreateRole(role);
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", roleResult.Errors));
                }
            }

            var user = this.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var createResult = this.Create(user, password);
                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", createResult.Errors));
                }

                var setLockoutResult = this.SetLockoutEnabled(user.Id, false);
                if (!setLockoutResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", setLockoutResult.Errors));
                }
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = this.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var addToRoleResult = this.AddToRole(user.Id, role.Name);
                if (!addToRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", addToRoleResult.Errors));
                }
            }
        }

        private void createApplicationUserManager()
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<ApplicationUser, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            this.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser, int>
            {
                MessageFormat = "Your security code is: {0}"
            });
            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser, int>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });

            if (_dataProtectionProvider != null)
            {
                var dataProtector = _dataProtectionProvider.Create("ASP.NET Identity");
                this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtector);
            }
        }
    }
}