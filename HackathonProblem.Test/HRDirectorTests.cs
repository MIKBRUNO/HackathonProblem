using HackathonProblem.Implementations;

namespace HackathonProblem.Test;

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

    [Fact]
    public void CalculateSatisfaction_WithPredeterminedRatings_Test1()
    {
        // best teams
        IEnumerable<ITeam> teams = [
            new Team(TestEmployees.teamleads[0], TestEmployees.juniors[0]),
            new Team(TestEmployees.teamleads[1], TestEmployees.juniors[1]),
            new Team(TestEmployees.teamleads[2], TestEmployees.juniors[2])
        ];
        var director = new HRDirector();
        double actual = director.CalculateSatisfaction(teams, TestEmployees.teamleadsWishlists, TestEmployees.juniorssWishlists);
        Assert.Equal(3, actual, 12);
    }

    [Fact]
    public void CalculateSatisfaction_WithPredeterminedRatings_Test2()
    {
        // second for teamleads, worst for juniors
        // [2]*3 + [1]*3
        IEnumerable<ITeam> teams = [
            new Team(TestEmployees.teamleads[0], TestEmployees.juniors[1]),
            new Team(TestEmployees.teamleads[1], TestEmployees.juniors[2]),
            new Team(TestEmployees.teamleads[2], TestEmployees.juniors[0])
        ];
        var director = new HRDirector();
        double actual = director.CalculateSatisfaction(teams, TestEmployees.teamleadsWishlists, TestEmployees.juniorssWishlists);
        Assert.Equal(4.0 / 3.0, actual, 12);
    }

    [Fact]
    public void CalculateSatisfaction_WithPredeterminedRatings_Test3()
    {
        // 1, 2, 3, 3, 2, 1
        IEnumerable<ITeam> teams = [
            new Team(TestEmployees.teamleads[0], TestEmployees.juniors[2]),
            new Team(TestEmployees.teamleads[1], TestEmployees.juniors[1]),
            new Team(TestEmployees.teamleads[2], TestEmployees.juniors[0])
        ];
        var director = new HRDirector();
        double actual = director.CalculateSatisfaction(teams, TestEmployees.teamleadsWishlists, TestEmployees.juniorssWishlists);
        Assert.Equal(18.0 / 11.0, actual, 12);
    }
}
