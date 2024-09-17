using TeamBuilding;

namespace HackathonProblem.Implementations;

public class HRDirector : IHRDirector
{
    public double CalculateSatisfaction(
        IEnumerable<ITeam> teams, IEnumerable<IWishlist> teamleadsWishlists, IEnumerable<IWishlist> juniorsWishlists)
    {
        var teamleadsPreferences = teamleadsWishlists.Select(
            w => new Preferences<IEmployee>(w.Owner, w.DesiredEmployees)
        ).ToDictionary(p => p.Owner);
        var juniorsPreferences = juniorsWishlists.Select(
            w => new Preferences<IEmployee>(w.Owner, w.DesiredEmployees)
        ).ToDictionary(p => p.Owner);
        double employeesCount = teamleadsPreferences.Count + juniorsPreferences.Count;
        double sum = 0;
        foreach (var team in teams)
        {
            sum += 1.0 / teamleadsPreferences[team.Teamlead].GetRating(juniorsPreferences[team.Junior].Owner);
            sum += 1.0 / juniorsPreferences[team.Junior].GetRating(teamleadsPreferences[team.Teamlead].Owner);
        }
        return employeesCount / sum;
    }
}
