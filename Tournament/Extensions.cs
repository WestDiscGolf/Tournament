using System.Linq;

namespace Tournament
{
    public static class Extensions
    {
        public static string Id(this string ravenId)
        {
            var split = ravenId.Split('/');
            return (split.Count() == 2) ? split[1] : split[0];
        }
    }
}