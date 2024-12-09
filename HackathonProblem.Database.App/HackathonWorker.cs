using CommandLine;
using HackathonProblem.Database.App.CommandLineOptions;
using HackathonProblem.Database.DataTypes;
using HackathonProblem.Database.EmployeeProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HackathonProblem.Database.App;

public class HackathonWorker(
    ParserResult<object> parserResult,
    ILogger<HackathonWorker> logger, IHostApplicationLifetime lifetime,
    HackathonContext context,
    ITeamleadsProvider teamleadsProvider, IJuniorsProvider juniorsProvider,
    IHackathon hackathon, IHRManager manager, IHRDirector director
    ) : IHostedService
{
    public async Task Perform(CancellationToken cancellationToken)
    {
        var juniors = context.Juniors;
        var teamleads = context.Teamleads;
        List<JuniorWishlist?>? juniorsWishlists = null;
        List<TeamleadWishlist?>? teamleadsWishlists = null;
        hackathon.OnWishlistGenerated += (_, p) =>
        {
            logger.LogDebug("Wishlists generated");
            juniorsWishlists = p.JuniorsWishlists.Select(w => w as JuniorWishlist).ToList();
            teamleadsWishlists = p.TeamleadsWishlists.Select(w => w as TeamleadWishlist).ToList();
        };
        logger.LogDebug("Hackathon started");
        var result = hackathon.Perform(teamleads, juniors, manager, director);
        logger.LogDebug("Hackathon finished");
        await context.Hackathons.AddAsync(new Hackathon() {
            Id = 0,
            JuniorWishlists = juniorsWishlists!,
            TeamleadWishlists = teamleadsWishlists!,
            Teams = result.Teams.Select(t => t as Team).ToList()!,
            StisfactionRate = result.SatisfactionRate
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        logger.LogDebug("Hackathon saved");
        logger.LogDebug("Stopping application");
        lifetime.StopApplication();
    }

    public async Task Show(ShowOptions options, CancellationToken cancellationToken)
    {
        if (options.Employees)
        {
            Console.WriteLine("{0,25}", "-=Employees=-");
            var juniors = context.Juniors;
            var teamleads = context.Teamleads;
            Console.WriteLine("Teamleads");
            foreach (var t in teamleads)
                Console.WriteLine($"{t.Id}. {t.Name}");
            Console.WriteLine();
            Console.WriteLine("Juniors");
            foreach (var j in juniors)
                Console.WriteLine($"{j.Id}. {j.Name}");
            Console.WriteLine();
        }
        Hackathon hackathon;
        if (options.Wishlists)
        {
            hackathon = await context.Hackathons
                .Where(h => h.Id == options.Id)
                .Include(h => h.JuniorWishlists)
                    .ThenInclude(t => t.Ratings)
                    .ThenInclude(r => r.Mate)
                .Include(h => h.TeamleadWishlists)
                    .ThenInclude(t => t.Ratings)
                    .ThenInclude(r => r.Mate)
                .AsSplitQuery()
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new ArgumentException("Bad hackathon id");
            Console.WriteLine("{0,25}", "-=Wishlists=-");
            var tw = hackathon.TeamleadWishlists;
            var jw = hackathon.JuniorWishlists;
            foreach (var t in tw)
            {
                Console.WriteLine($"Teamlead {t.Owner.Name}'s Wishlist:");
                foreach (var r in t.Ratings.OrderBy(r => r.Rating))
                    Console.WriteLine($"{r.Rating}: {r.Mate.Name}");
            }
            foreach (var j in jw)
            {
                Console.WriteLine($"Junior {j.Owner.Name}'s Wishlist:");
                foreach (var r in j.Ratings.OrderBy(r => r.Rating))
                    Console.WriteLine($"{r.Rating}: {r.Mate.Name}");
            }
            Console.WriteLine();
        }
        hackathon = await context.Hackathons
            .Where(h => h.Id == options.Id)
            .Include(h => h.Teams)
                .ThenInclude(t => t.Teamlead)
            .Include(h => h.Teams)
                .ThenInclude(t => t.Junior)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException("Bad hackathon id");
        var teams = hackathon.Teams;
        Console.WriteLine("{0,25}", "-=Teams=-");
        Console.WriteLine("{0,-25}{1,-25}", "Teamleads", "Juniors");
        foreach (var t in teams)
            Console.WriteLine("{0,-25}{1,-25}", t.Teamlead.Name, t.Junior.Name);
        Console.WriteLine("{0,25}", "-=Staisfaction Rate=-");
        Console.WriteLine();
        Console.WriteLine("{0,-25}{1:N2}", "Satisfaction rate:", hackathon.StisfactionRate);
        logger.LogDebug("Stopping application");
        lifetime.StopApplication();
    }

    public async Task Average(CancellationToken cancellationToken)
    {
        var avg = await context.Hackathons
            .Select(h => h.StisfactionRate)
            .AverageAsync(cancellationToken);
        Console.WriteLine($"Average satisfaction rate: {avg}");
        logger.LogDebug("Stopping application");
        lifetime.StopApplication();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await PrepareDatabse(cancellationToken);
        logger.LogDebug("Application started");
        try
        {
            await parserResult.MapResult(
                (PerformOptions o) => Perform(TokenSource.Token),
                (ShowOptions o) => Show(o, TokenSource.Token),
                (AverageOptions o) => Average(TokenSource.Token),
                errs => Task.Run(() => {})
            );
            logger.LogDebug("Application finished");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occured: {e.Message}");
        }
        finally
        {
            logger.LogDebug("Stopping application");
            lifetime.StopApplication();
        }
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await TokenSource.CancelAsync();
        logger.LogDebug("Application stopped");
    }

    private readonly CancellationTokenSource TokenSource = new();

    private async Task PrepareDatabse(CancellationToken cancellationToken)
    {
        logger.LogDebug("Preparing database");
        await context.Database.EnsureCreatedAsync(cancellationToken);
        logger.LogTrace("Database created");
        Task<bool>[] tasks = [
            context.Juniors.AnyAsync(cancellationToken),
            context.Teamleads.AnyAsync(cancellationToken),
        ];
        await Task.WhenAll(tasks);
        if (!tasks.All(t => t.Result))
        {
            logger.LogTrace("Users not found");
            logger.LogTrace("Uploading users");
            await Task.WhenAll([
                context.Juniors.AddRangeAsync(juniorsProvider.GetJuniors(), cancellationToken),
                context.Teamleads.AddRangeAsync(teamleadsProvider.GetTeamleads(), cancellationToken),
            ]);
            await context.SaveChangesAsync(cancellationToken);
            logger.LogTrace("Users uploaded");
        }
        logger.LogDebug("Database ready");
    }
}
