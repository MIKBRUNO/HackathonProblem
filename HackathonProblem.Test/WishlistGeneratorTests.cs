using HackathonProblem.Implementations;

namespace HackathonProblem.Test;

public abstract class AWishlistGeneratorTests
{
    protected abstract IWishlistGenerator Generator { get; }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void GenerateWishlist_InputIsFewEmployees_SizeMustBeSame(int employeesCount)
    {
        var employees = from i in Enumerable.Range(0, employeesCount)
        select new Employee(i, "");
        var employee = new Employee(-1, "");
        var wishlist = Generator.GenerateWishlist(employee, employees);

        Assert.Equal(employeesCount, wishlist.DesiredEmployees.Count());
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 10)]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 0)]
    [InlineData(10, 10)]
    public void GenerateWishlists_InputIsFewTeamleadsAndJuniors_SizesMustBeSame(int teamleadsCount, int juniorsCount)
    {
        var teamleads = from i in Enumerable.Range(0, teamleadsCount)
        select new Employee(i, "lead");
        var juniors = from i in Enumerable.Range(0, juniorsCount)
        select new Employee(i, "junior");
        var wishlists = Generator.GenerateWishlists(teamleads, juniors);

        Assert.Equal(teamleadsCount, wishlists.Count());
        foreach (var teamleadWishlist in wishlists)
        {
            Assert.Equal(juniorsCount, teamleadWishlist.DesiredEmployees.Count());
        }
    }

    [Fact]
    public void GenerateWishlist_InputIs10Employees_EveryoneMustBeInWishlist()
    {
        var employees = new List<IEmployee>(10);
        for (int i = 0; i < 10; ++i)
        {
            employees.Add(new Employee(i, $"Employee{i}"));
        }
        var wishlist = Generator.GenerateWishlist(new Employee(-1, ""), employees);

        foreach (var employee in employees)
        {
            Assert.Contains(employee, wishlist.DesiredEmployees);
        }
    }

    [Fact]
    public void GenerateWishlists_InputIs10Employees_EveryoneMustBeInWishlists()
    {
        var teamleads = new List<IEmployee>(10);
        var juniors = new List<IEmployee>(10);
        for (int i = 0; i < 10; ++i)
        {
            teamleads.Add(new Employee(i, $"Teamlead{i}"));
            juniors.Add(new Employee(i, $"Junior{i}"));
        }
        var wishlists = Generator.GenerateWishlists(teamleads, juniors);

        var wishlistOwners = from w in wishlists select w.Owner;
        foreach(var t in teamleads)
        {
            Assert.Contains(t, wishlistOwners);
        }
        foreach(var wishlist in wishlists)
        {
            foreach(var j in juniors)
            {
                Assert.Contains(j, wishlist.DesiredEmployees);
            }
        }
    }
}

public class RandomWishlistGeneratorTests : AWishlistGeneratorTests
{
    private readonly IWishlistGenerator randomWishlistGenerator
        = new RandomWishlistGenerator(new Random());

    protected override IWishlistGenerator Generator => randomWishlistGenerator;
}
