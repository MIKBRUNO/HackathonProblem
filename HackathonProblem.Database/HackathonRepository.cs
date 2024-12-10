using HackathonProblem.Database.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace HackathonProblem.Database;

public class HackathonRepository(HackathonContext context, IWishlistGenerator? generator = null) : IHackathonInfo, IHackathonRepository
{
    public IHackathonInfo PerformAndSave(
        IEnumerable<IEmployee> teamleads, IEnumerable<IEmployee> juniors,
        IHRManager manager, IHRDirector director)
    {
        if (generator is null) {
            throw new ArgumentException("Wishlist generator is required for Perform call");
        }
        EmployeeFactory<Junior> jFactory = new();
        EmployeeFactory<Teamlead> tFactory = new();
        var dbTeamleads = teamleads.Select(t => tFactory.CreateEmployee(t.Id, t.Name) as Teamlead).ToList();
        var dbJuniors = teamleads.Select(t => jFactory.CreateEmployee(t.Id, t.Name) as Junior).ToList();
        var teamleadsWishlists = generator.GenerateWishlists(dbTeamleads!, dbJuniors!)
            .Select(w => w as TeamleadWishlist).ToList();
        var juniorsWishlists = generator.GenerateWishlists(dbJuniors!, dbTeamleads!)
            .Select(w => w as JuniorWishlist).ToList();
        var teams = manager.BuildTeams(teamleadsWishlists!, juniorsWishlists!).Select(t => t as Team).ToList();
        double rate = director.CalculateSatisfaction(teams!, teamleadsWishlists!, juniorsWishlists!);
        Hackathon h = new() {
            Teamleads = dbTeamleads!,
            Juniors = dbJuniors!,
            TeamleadWishlists = teamleadsWishlists!,
            JuniorWishlists = juniorsWishlists!,
            Teams = teams!,
            StisfactionRate = rate,
        };
        context.Hackathons.Add(h);
        context.SaveChanges();
        _hackathon = h;
        return this;
    }

    public IHackathonInfo Load(int id)
    {
        _id = id;
        _hackathon = null;
        return this;
    }

    public double OverallAverageScore() =>
        context.Hackathons
            .Select(h => h.StisfactionRate)
            .Average();

    public int Id => _hackathon?.Id ?? _id;

    public IEnumerable<IWishlist> TeamleadWishlists => _hackathon?.TeamleadWishlists ?? 
        context.Hackathons
            .Where(h => h.Id == _id)
            .Include(h => h.TeamleadWishlists)
                .ThenInclude(t => t.Ratings)
                .ThenInclude(r => r.Mate)
            .AsSplitQuery()
            .SingleOrDefault()?
            .TeamleadWishlists
            ?? throw new ArgumentException("Bad hackathon id");

    public IEnumerable<IWishlist> JuniorWishlists => _hackathon?.JuniorWishlists ?? 
        context.Hackathons
            .Where(h => h.Id == _id)
            .Include(h => h.JuniorWishlists)
                .ThenInclude(t => t.Ratings)
                .ThenInclude(r => r.Mate)
            .AsSplitQuery()
            .SingleOrDefault()?
            .JuniorWishlists
            ?? throw new ArgumentException("Bad hackathon id");

    public IEnumerable<ITeam> Teams => _hackathon?.Teams ?? 
        context.Hackathons
            .Where(h => h.Id == _id)
            .Include(h => h.Teams)
                .ThenInclude(t => t.Teamlead)
            .Include(h => h.Teams)
                .ThenInclude(t => t.Junior)
            .AsSplitQuery()
            .SingleOrDefault()?
            .Teams
            ?? throw new ArgumentException("Bad hackathon id");

    public double StisfactionRate => _hackathon?.StisfactionRate ??
        context.Hackathons
            .Where(h => h.Id == _id)
            .SingleOrDefault()?
            .StisfactionRate
            ?? throw new ArgumentException("Bad hackathon id");

    public IEnumerable<IEmployee> Teamleads => _hackathon?.Teamleads ?? 
        context.Hackathons
            .Where(h => h.Id == _id)
            .Include(h => h.Teamleads)
            .AsSplitQuery()
            .SingleOrDefault()?
            .Teamleads
            ?? throw new ArgumentException("Bad hackathon id");

    public IEnumerable<IEmployee> Juniors => _hackathon?.Juniors ?? 
        context.Hackathons
            .Where(h => h.Id == _id)
            .Include(h => h.Juniors)
            .AsSplitQuery()
            .SingleOrDefault()?
            .Juniors
            ?? throw new ArgumentException("Bad hackathon id");

    private Hackathon? _hackathon;
    private int _id;
}
