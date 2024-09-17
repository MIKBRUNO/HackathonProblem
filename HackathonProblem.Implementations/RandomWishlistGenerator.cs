using HackathonProblem.Implementations.Utils;

namespace HackathonProblem.Implementations;

public class RandomWishlistGenerator(Random random) : IWishlistGenerator
{
    private readonly RandomShuffler<IEmployee> shuffler = new(random);

    public IWishlist GenerateWishlist(IEmployee employee, IEnumerable<IEmployee> possibleTeammates)
    {
        return new Wishlist(employee, shuffler.Shuffle(possibleTeammates));
    }
}
