using System.Web.Mvc;
using System.Web.Routing;

namespace Tournament
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // add the entity detail route which will run from a single int id to access the details
            // this will allow for the route to be /team/1/team-name-slug
            routes.MapRoute(
                name: "EntityDetail",
                url: "{controller}/{id}/{slug}",
                defaults: new { action = "detail", slug = UrlParameter.Optional },
                constraints: new { id = @"^[0-9]+$" }
                );

            // this is the default route which allows for the ravendb id creation to be allowed for admin functions
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{*id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}