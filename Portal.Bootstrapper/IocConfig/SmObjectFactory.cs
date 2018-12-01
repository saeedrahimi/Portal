using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using Microsoft.Owin.Security;
using Portal.Core.Data;
using Portal.Core.Data.Repository;
using Portal.Core.Logging;
using Portal.Core.Service.Bulletin;
using Portal.Core.Service.Company;
using Portal.Data.Context;
using Portal.Data.Identity.Contracts;
using Portal.Data.Identity.Models;
using Portal.Data.Repository;
using Portal.Data.Repository.Bulletin;
using Portal.Data.Repository.Company;
using Portal.Logging.NLogLogger;
using Portal.Service.Bulletin;
using Portal.Service.Company;
using Portal.Service.Identity;
using StructureMap;

namespace Portal.Bootstrapper.IocConfig
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            return new Container(ioc =>
            {
                ioc.For<IIdentity>()
                    .Transient()
                    .Use(() => getIdentity());
                ioc.For<ISqlDbContext>().Transient().Use<SqlDbContext>();
                ioc.For<ILogger>().Singleton().Use<NLogLogger>();

                //ioc.For(typeof(IRepository<>)).ContainerScoped().Use(typeof(BaseRepository<>)).Transient();
                ioc.For<ICompanyGroupRepository>().Transient().Use<CompanyGroupRepository>();
                ioc.For<ICompanyGroupService>().Transient().Use<CompanyGroupService>();

                ioc.For<IBulletinTypeRepository>().Transient().Use<BulletinTypeRepository>();
                ioc.For<IBulletinTypeService>().Transient().Use<BulletinTypeService>();
                #region Identity
                ioc.For<IUserStore<ApplicationUser, int>>()
                   .Transient()
                   .Use<CustomUserStore>();

                ioc.For<IRoleStore<CustomRole, int>>()
                    .Transient()
                    .Use<CustomRoleStore>();

                ioc.For<IApplicationSignInManager>()
                      .Transient()
                      .Use<ApplicationSignInManager>();

                ioc.For<IApplicationRoleManager>()
                      .Transient()
                      .Use<ApplicationRoleManager>();

                ioc.For<IAuthenticationManager>()
                    .Transient()
                    .Use(() => HttpContext.Current.GetOwinContext().Authentication);

                ioc.For<IApplicationUserManager>()
                    .Transient()
                    .Use<ApplicationUserManager>();

                ioc.For<ApplicationUserManager>()
                   .Transient()
                   .Use(context => (ApplicationUserManager)context.GetInstance<IApplicationUserManager>());

                ioc.For<ICustomRoleStore>()
                      .Transient()
                      .Use<CustomRoleStore>();

                ioc.For<ICustomUserStore>()
                      .Transient()
                      .Use<CustomUserStore>();
                #endregion
            });
        }

        private static IIdentity getIdentity()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                return HttpContext.Current.User.Identity;
            }

            return ClaimsPrincipal.Current != null ? ClaimsPrincipal.Current.Identity : null;
        }
    }
}
