namespace HackathonProblem;

public interface IWishlist
{
    IEmployee Owner { get; }
    IEnumerable<IEmployee> DesiredEmployees { get; }
}

public record class Wishlist(IEmployee Owner, IEnumerable<IEmployee> DesiredEmployees) : IWishlist;
