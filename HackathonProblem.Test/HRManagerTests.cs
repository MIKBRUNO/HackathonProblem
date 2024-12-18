using HackathonProblem.TeamBuilding;
using HackathonProblem.Implementations;
using HackathonProblem.TeamBuilding.Algorithms;

using Moq;
using System.Linq.Expressions;
using HackathonProblem.Default;

namespace HackathonProblem.Test;


// try find fixtures for xUnit
public abstract class AHRManagerTests
{
    protected abstract IHRManager GetHRManager(
        ITeamBuildingAlgorithm<IEmployee> teamBuilding,
        IPreferencesFactory<IEmployee> factory);

    protected abstract ITeamBuildingAlgorithm<IEmployee> Algorithm { get; }

    protected abstract IEnumerable<IWishlist> PredefinedTeamleadsWishlists { get; }
    
    protected abstract IEnumerable<IWishlist> PredefinedJuniorsWishlists { get; }

    protected abstract IEnumerable<ITeam> PredefinedTeams { get; }

    private readonly IWishlistGenerator generator = new RandomWishlistGenerator(new Random(), new WishlistFactory());

    private Tuple<IEnumerable<IWishlist>, IEnumerable<IWishlist>> RandomWishslists(int count)
    {
        var employees1 = Enumerable.Range(0, count).Select(i => new Employee(i, "")).ToList();
        var employees2 = Enumerable.Range(0, count).Select(i => new Employee(i, "")).ToList();
        var wishlists1 = generator.GenerateWishlists(employees1, employees2);
        var wishlists2 = generator.GenerateWishlists(employees2, employees1);
        return new (wishlists1, wishlists2);
    }

    [Fact]
    public void BuildTeams_InputIsEmpty_StrategyMusctBeCalledOnce()
    {
        var mockAlgorithm = new Mock<ITeamBuildingAlgorithm<IEmployee>>();
        Expression<Func<ITeamBuildingAlgorithm<IEmployee>, IEnumerable<Pair<IEmployee>>>>
        expr = s => s.BuildPairs(
                    It.IsAny<IEnumerable<IPreferences<IEmployee>>>(),
                    It.IsAny<IEnumerable<IPreferences<IEmployee>>>());
        mockAlgorithm.Setup(expr).Returns([]);
        var manager = GetHRManager(mockAlgorithm.Object, new PreferencesFactory<IEmployee>());
        manager.BuildTeams([], []);
        var exception = Record.Exception(() => mockAlgorithm.Verify(expr, Times.Once));
        Assert.Null(exception);
    }

    [Fact]
    public void BuildTeams_InputIsRandom5x5Employees_StrategyMusctBeCalledOnce()
    {
                var mockAlgorithm = new Mock<ITeamBuildingAlgorithm<IEmployee>>();
        Expression<Func<ITeamBuildingAlgorithm<IEmployee>, IEnumerable<Pair<IEmployee>>>>
        expr = s => s.BuildPairs(
                    It.IsAny<IEnumerable<IPreferences<IEmployee>>>(),
                    It.IsAny<IEnumerable<IPreferences<IEmployee>>>());
        mockAlgorithm.Setup(expr).Returns([]);
        var manager = GetHRManager(mockAlgorithm.Object, new PreferencesFactory<IEmployee>());
        var (wishlists1, wishlists2) = RandomWishslists(5);
        manager.BuildTeams(wishlists1, wishlists2);
        var exception = Record.Exception(() => mockAlgorithm.Verify(expr, Times.Once));
        Assert.Null(exception);
    }

    [Fact]
    public void BuildTeams_InputIs5x5_ResultTeamsCountMustBe5()
    {
        var manager = GetHRManager(Algorithm, new PreferencesFactory<IEmployee>());
        var (wishlists1, wishlists2) = RandomWishslists(5);
        var teams = manager.BuildTeams(wishlists1, wishlists2);
        Assert.Equal(5, teams.Count());
    }

    [Fact]
    public void BuildTeams_InputIs0x0_ResultTeamsCountMustBe0()
    {
        var manager = GetHRManager(Algorithm, new PreferencesFactory<IEmployee>());
        var teams = manager.BuildTeams([], []);
        Assert.Empty(teams);
    }

    [Fact]
    public void BuildTeams_InputIsPredefined_ResultMustBePredefined()
    {
        var manager = GetHRManager(Algorithm, new PreferencesFactory<IEmployee>());
        var teams = manager.BuildTeams(PredefinedTeamleadsWishlists, PredefinedJuniorsWishlists);
        Assert.Equal(PredefinedTeams, teams);
    }
}

public class GaleShapleyHRManagerTests : AHRManagerTests
{
    protected override IHRManager GetHRManager(ITeamBuildingAlgorithm<IEmployee> teamBuilding, IPreferencesFactory<IEmployee> factory)
        => new HRManager(teamBuilding, factory, new TeamFactory());

    protected override ITeamBuildingAlgorithm<IEmployee> Algorithm
        => new GaleShapleyAlgorithm<IEmployee>();

    protected override IEnumerable<IWishlist> PredefinedTeamleadsWishlists
        => TestEmployees.teamleadsWishlists;

    protected override IEnumerable<IWishlist> PredefinedJuniorsWishlists
        => TestEmployees.juniorssWishlists;

    protected override IEnumerable<ITeam> PredefinedTeams
        => [
            new Team(TestEmployees.teamleads[0], TestEmployees.juniors[0]),
            new Team(TestEmployees.teamleads[1], TestEmployees.juniors[1]),
            new Team(TestEmployees.teamleads[2], TestEmployees.juniors[2]),
        ];
}

public class LPHRManagerTests : AHRManagerTests
{
    protected override IHRManager GetHRManager(ITeamBuildingAlgorithm<IEmployee> teamBuilding, IPreferencesFactory<IEmployee> factory)
        => new HRManager(teamBuilding, factory, new TeamFactory());

    protected override ITeamBuildingAlgorithm<IEmployee> Algorithm
        => new LPOptimizationAlgorithm<IEmployee>();

    protected override IEnumerable<IWishlist> PredefinedTeamleadsWishlists
        => TestEmployees.teamleadsWishlists;

    protected override IEnumerable<IWishlist> PredefinedJuniorsWishlists
        => TestEmployees.juniorssWishlists;

    protected override IEnumerable<ITeam> PredefinedTeams
        => [
            new Team(TestEmployees.teamleads[0], TestEmployees.juniors[0]),
            new Team(TestEmployees.teamleads[1], TestEmployees.juniors[1]),
            new Team(TestEmployees.teamleads[2], TestEmployees.juniors[2]),
        ];
}
