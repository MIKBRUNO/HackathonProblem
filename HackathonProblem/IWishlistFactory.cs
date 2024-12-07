namespace HackathonProblem;

public interface IWishlistFactory
{
    IWishlist CreateWishlist(IEmployee Owner, IEnumerable<IEmployee> DesiredEmployees);
}
