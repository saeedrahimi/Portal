using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Optimization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using Portal.Bootstrapper.IocConfig;
using Portal.Data.Context;
using Portal.Data.Identity.Contracts;
using Portal.Data.Identity.Models;
using Portal.Service.Identity;
using Portal.Web.Models;

namespace Portal.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        private static void ConfigureAuth(IAppBuilder app)
        {
            var container = SmObjectFactory.Container;
            container.Configure(config =>
            {
                config.For<IDataProtectionProvider>()
                      .Transient()
                      .Use(() => app.GetDataProtectionProvider());
            });
            container.GetInstance<IApplicationUserManager>().SeedDatabase();

            // This is necessary for `GenerateUserIdentityAsync` and `SecurityStampValidator` to work internally by ASP.NET Identity 2.x
            app.CreatePerOwinContext(() => (ApplicationUserManager)container.GetInstance<IApplicationUserManager>());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName = ".mySampleAppCookieName",
                ExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = context =>
                    {
                        if (shouldIgnoreRequest(context)) // How to ignore Authentication Validations for static files in ASP.NET Identity
                        {
                            return Task.FromResult(0);
                        }
                        return container.GetInstance<IApplicationUserManager>().OnValidateIdentity().Invoke(context);
                    }
                },
                SlidingExpiration = false,
                CookieManager = new SystemWebCookieManager()
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(
            //    clientId: "",
            //    clientSecret: "");

        }
        private static bool shouldIgnoreRequest(CookieValidateIdentityContext context)
        {
            string[] reservedPath =
            {
                "/__browserLink",
                "/img",
                "/fonts",
                "/Scripts",
                "/Content",
                "/Uploads",
                "/Images"
            };
            return reservedPath.Any(path => context.OwinContext.Request.Path.Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)) ||
                   BundleTable.Bundles.Select(bundle => bundle.Path.TrimStart('~')).Any(bundlePath => context.OwinContext.Request.Path.Value.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase));
        }
    }
}