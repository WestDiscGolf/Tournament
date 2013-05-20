using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Tournament.Infrastructure;

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

            // setup which store to use
            if (Settings.UseEmbedded)
            {
                Store = new EmbeddableDocumentStore { ConnectionStringName = "RavenDBEmbedded" };
            }
            else
            {
                Store = new DocumentStore { ConnectionStringName = "RavenDB" };                
            }

            // initialise the store
            Store.Initialize();

            // setup the indexs in the calling assembly on the configured store
            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), Store);

            if (!Settings.UseEmbedded)
            {
                // sort out the raven profiler
                Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(Store);   
            }            
        }
    }
}