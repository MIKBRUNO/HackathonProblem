using HackathonProblem.TeamBuilding.Algorithms;
using Nsu.HackathonProblem.Contracts;

namespace HackathonProblem.TeamBuilding;

public class GaleShapleyStrategy : ITeamBuildingStrategy
{
    private static IEnumerable<IPreferences<Employee>> WishlistsToPreferences(
        IDictionary<int, Employee> wishers,
        IDictionary<int, Employee> wishedEmployees,
        IEnumerable<Wishlist> wishlists)
    {
        return from wishlist in wishlists
            select new Preferences<Employee>(
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
        
        var pairs = new GaleShapleyAlgorithm<Employee>().BuildPairs(teamleadsPreferences, juniorsPreferences);
        return pairs.Select(pair => new Team(pair.Teamlead, pair.Junior));
    }
}
