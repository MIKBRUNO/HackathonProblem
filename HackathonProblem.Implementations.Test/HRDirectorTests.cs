namespace HackathonProblem.Implementations.Test;

public class HRDirectorTests
{
    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(14)]
    public void HarmonicMean_InputIsSequenceOfDoubles_ResultMustBeSame(double val)
    {
        IEnumerable<double> doubles = Enumerable.Range(0, 10).Select(_ => val).ToList();
        double actual = doubles.HarmonicMean();
        Assert.Equal(val, actual, 12);
    }

    [Fact]
    public void HarmonicMean_InputIsEmpty_ResultMustBeZero()
    {
        IEnumerable<double> doubles = [];
        double actual = doubles.HarmonicMean();
        Assert.Equal(0, actual, 12);
    }

    [Fact]
    public void HarmonicMean_InputIsNull_ResultMustBeZero()
    {
        IEnumerable<double>? doubles = null;
        double actual = doubles.HarmonicMean();
        Assert.Equal(0, actual, 12);
    }

    [Fact]
    public void HarmonicMean_InputIs2_6_ResultMust3()
    {
        IEnumerable<double> doubles = [2, 6];
        double actual = doubles.HarmonicMean();
        Assert.Equal(3, actual, 12);
    }

    [Fact]
    public void HarmonicMean_InputIs12_20_1_5_3_ResultMust3()
    {
        IEnumerable<double> doubles = [12, 20, 1, 5, 3];
        double actual = doubles.HarmonicMean();
        Assert.Equal(3, actual, 12);
    }

    private static readonly IList<IEmployee> teamleads = [
        new Employee(1, "UnoT"),
        new Employee(2, "DosT"),
        new Employee(3, "TresT")
    ];

    private static readonly IList<IEmployee> juniors = [
        new Employee(4, "UnoJ"),
        new Employee(5, "DosJ"),
        new Employee(6, "TresJ")
    ];

    private static readonly IEnumerable<IWishlist> teamleadsWishlists = [
        new Wishlist(teamleads[0], [juniors[0], juniors[1], juniors[2]]),
        new Wishlist(teamleads[1], [juniors[1], juniors[2], juniors[0]]),
        new Wishlist(teamleads[2], [juniors[2], juniors[0], juniors[1]])
    ];

    private static readonly IEnumerable<IWishlist> juniorssWishlists = [
        new Wishlist(juniors[0], [teamleads[0], teamleads[1], teamleads[2]]),
        new Wishlist(juniors[1], [teamleads[1], teamleads[2], teamleads[0]]),
        new Wishlist(juniors[2], [teamleads[2], teamleads[0], teamleads[1]])
    ];

    [Fact]
    public void CalculateSatisfaction_WithPredeterminedRatings_Test1()
    {
        // best teams
        IEnumerable<ITeam> teams = [
            new Team(teamleads[0], juniors[0]),
            new Team(teamleads[1], juniors[1]),
            new Team(teamleads[2], juniors[2])
        ];
        var director = new HRDirector();
        double actual = director.CalculateSatisfaction(teams, teamleadsWishlists, juniorssWishlists);
        Assert.Equal(3, actual, 12);
    }

    [Fact]
    public void CalculateSatisfaction_WithPredeterminedRatings_Test2()
    {
        // second for teamleads, worst for juniors
        // [2]*3 + [1]*3
        IEnumerable<ITeam> teams = [
            new Team(teamleads[0], juniors[1]),
            new Team(teamleads[1], juniors[2]),
            new Team(teamleads[2], juniors[0])
        ];
        var director = new HRDirector();
        double actual = director.CalculateSatisfaction(teams, teamleadsWishlists, juniorssWishlists);
        Assert.Equal(4.0 / 3.0, actual, 12);
    }

    [Fact]
    public void CalculateSatisfaction_WithPredeterminedRatings_Test3()
    {
        // 1, 2, 3, 3, 2, 1
        IEnumerable<ITeam> teams = [
            new Team(teamleads[0], juniors[2]),
            new Team(teamleads[1], juniors[1]),
            new Team(teamleads[2], juniors[0])
        ];
        var director = new HRDirector();
        double actual = director.CalculateSatisfaction(teams, teamleadsWishlists, juniorssWishlists);
        Assert.Equal(18.0 / 11.0, actual, 12);
    }
}
