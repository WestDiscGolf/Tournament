using System.Linq;
using Raven.Client.Indexes;
using Tournament.Entities;

namespace Tournament.Infrastructure.Indexes
{
    public class Extras_ByPlayer : AbstractIndexCreationTask<Extra>
    {
        public Extras_ByPlayer()
        {
            Map = extras => from extra in extras
                            select new Extra
                                {
                                    PlayerId = extra.PlayerId
                                };
        }
    }
}