using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Portal.Core.Logging;
using Portal.Logging.NLogLogger;
using StructureMap;
using StructureMap.Graph;

namespace Portal.Bootstrapper.IocConfig
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors  

        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new DefaultConventionScanner());
                });


            For<ILogger>().Use<NLogLogger>();

        }

        #endregion
    }
}
