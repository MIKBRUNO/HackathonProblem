using HackathonProblem.Implementations;
using HackathonProblem.TeamBuilding;
using HackathonProblem.TeamBuilding.Algorithms;

namespace HackathonProblem.Test;

public abstract class AHackthonTests
{   
    protected abstract IHackathon GetHackathon(IWishlistGenerator wishlistGenerator);

    protected abstract IHRManager Manager { get; }

    protected abstract IHRDirector Director { get; }

    private class PredefinedWishlistGenerator : IWishlistGenerator
    {
        public IWishlist GenerateWishlist(IEmployee employee, IEnumerable<IEmployee> possibleTeammates)
        {
            if (TestEmployees.juniors.Contains(employee))
            {
                return TestEmployees.juniorssWishlistsDict[employee];
            }
            else
            {
                return TestEmployees.teamleadsWishlistsDict[employee];
            }
        }
    }


// ramake as integration test with mocking
    [Fact]
    public void Perform_InputIsPredefined_RatingMustBePredefined()
    {
        var hackathon = GetHackathon(new PredefinedWishlistGenerator());
        var result = hackathon.Perform(
            TestEmployees.teamleads, TestEmployees.juniors,
            Manager, Director
        );
        Assert.Equal(3.0, result.SatisfactionRate);
    }
}

class GaleShapleyHackathonTests : AHackthonTests
{
    protected override IHRManager Manager => new HRManager(
        new GaleShapleyAlgorithm<IEmployee>(),
        new PreferencesFactory<IEmployee>()
    );

    protected override IHRDirector Director => new HRDirector();

    protected override IHackathon GetHackathon(IWishlistGenerator wishlistGenerator)
    {
        return new Hackathon(wishlistGenerator);
    }
}
