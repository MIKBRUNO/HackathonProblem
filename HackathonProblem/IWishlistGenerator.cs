namespace HackathonProblem;

public interface IWishlistGenerator
{
    /// <summary>
    /// Determines wishlists for employee on set of other employees
    /// </summary>
    /// <param name="employee"></param>
    /// <param name="possibleTeammates"></param>
    /// <returns>wishlists</returns>
    public IWishlist GenerateWishlist(
        IEmployee employee, IEnumerable<IEmployee> possibleTeammates);

    public IEnumerable<IWishlist> GenerateWishlists(IEnumerable<IEmployee> employees, IEnumerable<IEmployee> possibleTeammates)
    {
        List<IWishlist> wishlists = [];
        foreach (var employee in employees)
        {
            wishlists.Add(GenerateWishlist(employee, possibleTeammates));
        }
        return wishlists;
    }
}
