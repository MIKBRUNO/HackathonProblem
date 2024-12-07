namespace HackathonProblem.Default;

public record class Wishlist(IEmployee Owner, IEnumerable<IEmployee> DesiredEmployees) : IWishlist;

public class WishlistFactory : IWishlistFactory
{
    public IWishlist CreateWishlist(IEmployee Owner, IEnumerable<IEmployee> DesiredEmployees) => new Wishlist(Owner, DesiredEmployees);
}
