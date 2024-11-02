using Nsu.HackathonProblem.Contracts;

namespace HackathonProblem.TeamBuilding;

public class CommonAlgorithmStrategy<T> : ITeamBuildingStrategy where T: ITeamBuildingAlgorithm<Employee>, new()
{
    private static IEnumerable<IPreferences<Employee>> WishlistsToPreferences(
        IDictionary<int, Employee> wishers,
        IDictionary<int, Employee> wishedEmployees,
        IEnumerable<Wishlist> wishlists)
    {
        var preferencesFactory = new PreferencesFactory<Employee>();
        return from wishlist in wishlists
            select preferencesFactory.CreatePreferences(
                wishers[wishlist.EmployeeId],
                from id in wishlist.DesiredEmployees
                select wishedEmployees[id]
            );
    }

    public IEnumerable<Team> BuildTeams(
        IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists)
    {
        IDictionary<int, Employee> teamleadsDict = teamLeads.ToDictionary(e => e.Id);
        IDictionary<int, Employee> juniorsDict = juniors.ToDictionary(e => e.Id);
        IEnumerable<IPreferences<Employee>> teamleadsPreferences =
            WishlistsToPreferences(teamleadsDict, juniorsDict, teamLeadsWishlists);
        IEnumerable<IPreferences<Employee>> juniorsPreferences =
            WishlistsToPreferences(juniorsDict, teamleadsDict, juniorsWishlists);
        
        var pairs = new T().BuildPairs(teamleadsPreferences, juniorsPreferences);
        return pairs.Select(pair => new Team(pair.Teamlead, pair.Junior));
    }
}
