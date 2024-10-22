using HackathonProblem.TeamBuilding;

namespace HackathonProblem.Implementations;

public class HRManager(
    ITeamBuildingAlgorithm<IEmployee> algorithm,
    IPreferencesFactory<IEmployee> preferencesFactory) : IHRManager
{
    private readonly ITeamBuildingAlgorithm<IEmployee> algorithm = algorithm;
    private readonly IPreferencesFactory<IEmployee> preferencesFactory = preferencesFactory;

    public IEnumerable<ITeam> BuildTeams(
        IEnumerable<IWishlist> teamleadsWishlists, IEnumerable<IWishlist> juniorsWishlists)
    {
        var teamleadsPreferences = teamleadsWishlists.Select(
            w => preferencesFactory.CreatePreferences(w.Owner, w.DesiredEmployees)
        );
        var juniorsPreferences = juniorsWishlists.Select(
            w => preferencesFactory.CreatePreferences(w.Owner, w.DesiredEmployees)
        );
        var pairs = algorithm.BuildPairs(teamleadsPreferences, juniorsPreferences);
        return pairs.Select(p => new Team(p.Teamlead, p.Junior));
    }
}
