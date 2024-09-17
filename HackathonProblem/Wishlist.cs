namespace HackathonProblem;

public interface IWishlist
{
    IEmployee Owner { get; }
    IEnumerable<IEmployee> DesiredEmployees { get; }
}

public record Wishlist(IEmployee Owner, IEnumerable<IEmployee> DesiredEmployees) : IWishlist;
