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
    public class MvcApplication : HttpApplication
    {
        public static IDocumentStore Store;

        protected void Application_Start()
        {
            RegisterRavenDb();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            AutoMapperConfig.RegisterMappings();
            InfrastructureConfig.RegisterBinders();
            InfrastructureConfig.RegisterViewEngines();
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