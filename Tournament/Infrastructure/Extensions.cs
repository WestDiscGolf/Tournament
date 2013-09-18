using System;
using System.Linq;

namespace Tournament.Infrastructure
{
    public static class Extensions
    {
        public static string Id(this string ravenId)
        {
            var split = ravenId.Split('/');
            return (split.Count() == 2) ? split[1] : split[0];
        }

        public static T Id<T>(this string ravenId)
        {
            var idPart = ravenId.Id();
            return (T)Convert.ChangeType(idPart, typeof (T));
        }
    }
}