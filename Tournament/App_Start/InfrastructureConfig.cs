using System.Web.Mvc;
using Tournament.Infrastructure;

namespace Tournament
{
    public static class InfrastructureConfig
    {
        public static void RegisterBinders()
        {
            ModelBinders.Binders.Add(typeof(string[]), new StringArrayModelBinder());
        }

        public static void RegisterViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}