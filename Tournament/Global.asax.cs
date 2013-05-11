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
        public const string CurrentRequestRavenSessionKey = "CurrentRequestRavenSession";

        public static IDocumentStore Store;

        public MvcApplication()
        {
            BeginRequest += (sender, args) =>
            {
                HttpContext.Current.Items[CurrentRequestRavenSessionKey] = Store.OpenSession();
            };

            EndRequest += (sender, args) =>
            {
                using (var session = (IDocumentSession)HttpContext.Current.Items[CurrentRequestRavenSessionKey])
                {
                    if (session == null)
                        return;

                    if (Server.GetLastError() != null)
                        return;

                    session.SaveChanges();
                }
            };
        }

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
            // don't allow for misuse with this
            if (Store != null) return;

            // initialise the store
            Store = new DocumentStore { ConnectionStringName = "RavenDB" };
            Store.Initialize();

            // setup the indexs in the calling assembly
            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), Store);

            // sort out the raven profiler
            Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(Store);
        }
    }
}