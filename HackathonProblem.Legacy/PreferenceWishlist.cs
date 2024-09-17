using HackathonProblem.Base.Concepts;

namespace HackathonProblem.Base;

public record PreferenceWishlist : Wishlist, IPreference
{
    private Dictionary<int, int> Ratings = [];
    public PreferenceWishlist(Wishlist original) : base(original)
    {
        int MAX_RATING = DesiredEmployees.Count();
        int rating = MAX_RATING;
        foreach (int employeeId in DesiredEmployees)
        {
            Ratings[employeeId] = rating;
            ++rating;
        }
    }

    public int getEmployeeRating(int employeeId) => Ratings[employeeId];
}
