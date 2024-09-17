using HackathonProblem.Base.Concepts;

namespace HackathonProblem.Base;

public class SatisfactionCalculatorHarmonicMean : ISatisfactionCalculator
{
    public double CalculateSatisfaction(IEnumerable<Team> teams,
        IEnumerable<Wishlist> teamleadsWishlists, IEnumerable<Wishlist> juniorWishlists)
    {
        Dictionary<int, PreferenceWishlist> teamleadsPreferences = 
        (
            from wishlist in teamleadsWishlists
            select new PreferenceWishlist(wishlist)
        ).ToDictionary(p => p.EmployeeId);
        Dictionary<int, PreferenceWishlist> juniorsPreferences = 
        (
            from wishlist in juniorWishlists
            select new PreferenceWishlist(wishlist)
        ).ToDictionary(p => p.EmployeeId);

        double employeesCount = 0;
        double sumReciprocals = .0;
        foreach (Team team in teams)
        {
            sumReciprocals += 1.0 / teamleadsPreferences[team.TeamLead.Id].getEmployeeRating(team.Junior.Id);
            sumReciprocals += 1.0 / juniorsPreferences[team.Junior.Id].getEmployeeRating(team.TeamLead.Id);
            ++employeesCount;
        }
        return employeesCount / sumReciprocals;
    }
}
