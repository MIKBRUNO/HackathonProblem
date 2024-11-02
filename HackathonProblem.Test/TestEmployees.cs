namespace HackathonProblem.Test;

public sealed class TestEmployees
{
    public static readonly IList<IEmployee> teamleads = [
        new Employee(1, "UnoT"),
        new Employee(2, "DosT"),
        new Employee(3, "TresT")
    ];

    public static readonly IList<IEmployee> juniors = [
        new Employee(4, "UnoJ"),
        new Employee(5, "DosJ"),
        new Employee(6, "TresJ")
    ];

    public static readonly IEnumerable<IWishlist> teamleadsWishlists = [
        new Wishlist(teamleads[0], [juniors[0], juniors[1], juniors[2]]),
        new Wishlist(teamleads[1], [juniors[1], juniors[2], juniors[0]]),
        new Wishlist(teamleads[2], [juniors[2], juniors[0], juniors[1]])
    ];

    public static readonly IEnumerable<IWishlist> juniorssWishlists = [
        new Wishlist(juniors[0], [teamleads[0], teamleads[1], teamleads[2]]),
        new Wishlist(juniors[1], [teamleads[1], teamleads[2], teamleads[0]]),
        new Wishlist(juniors[2], [teamleads[2], teamleads[0], teamleads[1]])
    ];

    public static readonly IDictionary<IEmployee, IWishlist> teamleadsWishlistsDict
        = teamleadsWishlists.ToDictionary(w => w.Owner);

    public static readonly IDictionary<IEmployee, IWishlist> juniorssWishlistsDict
        = juniorssWishlists.ToDictionary(w => w.Owner);
}
