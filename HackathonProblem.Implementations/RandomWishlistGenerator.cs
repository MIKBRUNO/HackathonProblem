using HackathonProblem.Implementations.Utils;

namespace HackathonProblem.Implementations;

public class RandomWishlistGenerator(Random random, IWishlistFactory factory) : IWishlistGenerator
{
    private readonly RandomShuffler<IEmployee> shuffler = new(random);

    public IWishlist GenerateWishlist(IEmployee employee, IEnumerable<IEmployee> possibleTeammates)
    {
        return factory.CreateWishlist(employee, shuffler.Shuffle(possibleTeammates));
    }
}
