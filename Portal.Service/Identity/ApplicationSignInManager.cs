using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Portal.Data.Identity.Contracts;
using Portal.Data.Identity.Models;

namespace Portal.Service.Identity
{
    public class ApplicationSignInManager :
        SignInManager<ApplicationUser, int>,
        IApplicationSignInManager
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public ApplicationSignInManager(
            IApplicationUserManager userManager,
            IAuthenticationManager authenticationManager) :
            base((ApplicationUserManager)userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return _userManager.GenerateUserIdentityAsync(user);
        }

        /// <summary>
        /// How to refresh authentication cookies
        /// </summary>
        public async Task RefreshSignInAsync(ApplicationUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            // await _userManager.UpdateSecurityStampAsync(user.Id).ConfigureAwait(false); // = used for SignOutEverywhere functionality
            var claimsIdentity = await _userManager.GenerateUserIdentityAsync(user).ConfigureAwait(false);
            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, claimsIdentity);
        }
    }
}