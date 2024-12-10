using HackathonProblem.Database;
using HackathonProblem.Database.DataTypes;
using HackathonProblem.Implementations;
using HackathonProblem.TeamBuilding;
using HackathonProblem.TeamBuilding.Algorithms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;

namespace HackathonProblem.Test;

file class TestDbContextFactory(DbContextOptions<HackathonContext> options) : IDbContextFactory<HackathonContext>
{
    public HackathonContext CreateDbContext() => new(options);
}

public abstract class ADatabaseTests
{
    protected abstract IHackathonRepository GetHackathonRepository(HackathonContext context, IWishlistGenerator? generator=null);

    public ADatabaseTests()
    {
		var options = new DbContextOptionsBuilder<HackathonContext>()
            .UseSqlite("DataSource=file::memory:?cache=shared").Options;
		_contextFactory = new TestDbContextFactory(options);
    }
    protected IDbContextFactory<HackathonContext> _contextFactory;

	[Fact]
	public void PerformAndSave_InputIsPredefined_ResultMustBeValid()
	{
        var context = _contextFactory.CreateDbContext();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
		IHackathonRepository repository = GetHackathonRepository(context, new RandomWishlistGenerator(new Random(), new WishlistFactory(context)));
        var info = repository.PerformAndSave(
            TestEmployees.teamleads, TestEmployees.juniors,
            new HRManager(new GaleShapleyAlgorithm<IEmployee>(), new PreferencesFactory<IEmployee>(), new TeamFactory()),
            new HRDirector()
        );
        Assert.Equal(1, info.Id);
        Assert.Equal(TestEmployees.teamleads.Count, info.Teamleads.Count());
        Assert.Equal(TestEmployees.teamleads.Count, info.TeamleadWishlists.Count());
        Assert.Equal(TestEmployees.juniors.Count, info.Juniors.Count());
        Assert.Equal(TestEmployees.juniors.Count, info.JuniorWishlists.Count());
        context.Database.EnsureDeleted();
        context.Database.CloseConnection();
        context.Dispose();
	}

    [Fact]
	public void Load_ResultMustBeAsSaved()
	{
        var context = _contextFactory.CreateDbContext();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
		IHackathonRepository repository = GetHackathonRepository(context, new RandomWishlistGenerator(new Random(), new WishlistFactory(context)));
        var info1 = repository.PerformAndSave(
            TestEmployees.teamleads, TestEmployees.juniors,
            new HRManager(new GaleShapleyAlgorithm<IEmployee>(), new PreferencesFactory<IEmployee>(), new TeamFactory()),
            new HRDirector()
        );
        var info2 = GetHackathonRepository(context).Load(1);
        Assert.Equal(info1.Id, info2.Id);
        Assert.Equal(info1.Teamleads.Count(), info2.Teamleads.Count());
        Assert.Equal(info1.TeamleadWishlists.Count(), info2.TeamleadWishlists.Count());
        Assert.Equal(info1.Juniors.Count(), info2.Juniors.Count());
        Assert.Equal(info1.JuniorWishlists.Count(), info2.JuniorWishlists.Count());
        Assert.Equal(info1.Teams.Count(), info2.Teams.Count());
        Assert.Equal(info1.StisfactionRate, info2.StisfactionRate);
        context.Database.EnsureDeleted();
        context.Database.CloseConnection();
        context.Dispose();
	}

    [Fact]
	public void OverallAverageScore_ResultMustBeSameAsOnlyResult()
	{
        var context = _contextFactory.CreateDbContext();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
		IHackathonRepository repository = GetHackathonRepository(context, new RandomWishlistGenerator(new Random(), new WishlistFactory(context)));
        var info1 = repository.PerformAndSave(
            TestEmployees.teamleads, TestEmployees.juniors,
            new HRManager(new GaleShapleyAlgorithm<IEmployee>(), new PreferencesFactory<IEmployee>(), new TeamFactory()),
            new HRDirector()
        );
        var rate = GetHackathonRepository(context).OverallAverageScore();
        Assert.Equal(info1.StisfactionRate, rate);
        context.Database.EnsureDeleted();
        context.Database.CloseConnection();
        context.Dispose();
	}
}

public class DatabaseTests : ADatabaseTests
{
    protected override IHackathonRepository GetHackathonRepository(HackathonContext context, IWishlistGenerator? generator = null)
    {
        return new HackathonRepository(context, generator);
    }
}
