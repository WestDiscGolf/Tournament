using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Tournament
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static IDocumentStore Store;

        protected void Application_Start()
        {
            RegisterRazorViewEngine();
            RegisterRavenDb();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            AutoMapperConfig.RegisterMappings();
        }

        private static void RegisterRazorViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        private static void RegisterRavenDb()
        {
            Store = new DocumentStore { ConnectionStringName = "RavenDB" };
            Store.Initialize();
            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), Store);

            Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(Store);
        }
    }
}