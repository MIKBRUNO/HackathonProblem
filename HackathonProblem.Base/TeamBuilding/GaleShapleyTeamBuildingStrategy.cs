using HackathonProblem.Base.Concepts;
using HackathonProblem.Base.TeamBuilding.GaleShapley;

namespace HackathonProblem.Base.TeamBuilding;

public class GaleShapleyTeamBuildingStrategy : ITeamBuildingStrategy
{
    public IEnumerable<Team> BuildTeams(IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists)
    {
        Dictionary<int, Employee> dictTeamleads = [];
        foreach (Employee teamlead in teamLeads)
        {
            dictTeamleads[teamlead.Id] = teamlead;
        }
        Dictionary<int, Employee> dictJuniors = [];
        foreach (Employee junior in juniors)
        {
            dictTeamleads[junior.Id] = junior;
        }

        IEnumerable<(int, int)> pairs = GaleShapleyAlgorithm.FindManOptimalMarriage(juniorsWishlists, teamLeadsWishlists);
        IEnumerable<Team> teams = from pair in pairs
            select new Team(dictTeamleads[pair.Item2], dictJuniors[pair.Item1]);
        return teams;
    }
}
