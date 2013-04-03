using System.Linq;

namespace Tournament
{
    public static class Extensions
    {
        public static string Id(this string id)
        {
            var split = id.Split('/');
            return (split.Count() == 2) ? split[1] : split[0];
        }
    }
}