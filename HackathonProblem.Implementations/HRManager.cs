using HackathonProblem.TeamBuilding;

namespace HackathonProblem.Implementations;

public class HRManager(IMarriageAlgorithm<IEmployee> algorithm) : IHRManager
{
    private readonly IMarriageAlgorithm<IEmployee> algorithm = algorithm;

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
        return pairs.Select(p => new Team(p.Man, p.Woman));
    }
}
