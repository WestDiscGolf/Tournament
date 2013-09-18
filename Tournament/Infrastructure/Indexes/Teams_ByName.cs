using System.Linq;
using Raven.Client.Indexes;
using Tournament.Entities;

namespace Tournament.Infrastructure.Indexes
{
    public class Teams_ByName : AbstractIndexCreationTask<Team>
    {
        public Teams_ByName()
        {
            Map = teams => from team in teams
                           orderby team.Name
                           select new {team.Name};
        }
    }
}