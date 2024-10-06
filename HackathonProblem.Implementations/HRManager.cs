using HackathonProblem.TeamBuilding;

namespace HackathonProblem.Implementations;

public class HRManager(ITeamBuildingAlgorithm<IEmployee> algorithm) : IHRManager
{
    private readonly ITeamBuildingAlgorithm<IEmployee> algorithm = algorithm;

    public IEnumerable<ITeam> BuildTeams(
        IEnumerable<IWishlist> teamleadsWishlists, IEnumerable<IWishlist> juniorsWishlists)
    {
        var teamleadsPreferences = teamleadsWishlists.Select(
            w => new Preferences<IEmployee>(w.Owner, w.DesiredEmployees)
        );
        var juniorsPreferences = juniorsWishlists.Select(
            w => new Preferences<IEmployee>(w.Owner, w.DesiredEmployees)
        );
        var pairs = algorithm.BuildMarriage(teamleadsPreferences, juniorsPreferences);
        return pairs.Select(p => new Team(p.Teamlead, p.Junior));
    }
}
