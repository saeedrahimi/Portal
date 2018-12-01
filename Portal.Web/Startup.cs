using Microsoft.Owin;
using Owin;
using Portal.Bootstrapper.IocConfig;
using Portal.Data.Context;

[assembly: OwinStartupAttribute(typeof(Portal.Web.Startup))]
namespace Portal.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
