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
    IHackathonRepository repository, HackathonContext context,
    ITeamleadsProvider teamleadsProvider, IJuniorsProvider juniorsProvider,
    IHRManager manager, IHRDirector director
    ) : IHostedService
{
    public Task Perform(CancellationToken cancellationToken)
    {
        var juniors = juniorsProvider.GetEmployees();
        var teamleads = teamleadsProvider.GetEmployees();
        logger.LogDebug("Hackathon started");
        var info = repository.PerformAndSave(teamleads, juniors, manager, director);
        logger.LogDebug("Hackathon finished");
        Console.WriteLine($"Hackathon id: {info.Id}");
        Console.WriteLine($"Hackathon score: {info.StisfactionRate}");
        logger.LogDebug("Stopping application");
        lifetime.StopApplication();
        return Task.CompletedTask;
    }

    public Task Show(ShowOptions options, CancellationToken cancellationToken)
    {
        var info = repository.Load(options.Id);
        if (options.Employees)
        {
            Console.WriteLine("{0,25}", "-=Employees=-");
            var juniors = info.Juniors;
            var teamleads = info.Teamleads;
            Console.WriteLine("Teamleads");
            foreach (var t in teamleads)
                Console.WriteLine($"{t.Id}. {t.Name}");
            Console.WriteLine();
            Console.WriteLine("Juniors");
            foreach (var j in juniors)
                Console.WriteLine($"{j.Id}. {j.Name}");
            Console.WriteLine();
        }
        if (options.Wishlists)
        {
            Console.WriteLine("{0,25}", "-=Wishlists=-");
            var tw = info.TeamleadWishlists;
            var jw = info.JuniorWishlists;
            foreach (var t in tw)
            {
                Console.WriteLine($"Teamlead {t.Owner.Name}'s Wishlist:");
                foreach (var r in t.DesiredEmployees)
                    Console.WriteLine($"{r.Name}");
            }
            foreach (var j in jw)
            {
                Console.WriteLine($"Junior {j.Owner.Name}'s Wishlist:");
                foreach (var r in j.DesiredEmployees)
                    Console.WriteLine($"{r.Name}");
            }
            Console.WriteLine();
        }
        var teams = info.Teams;
        Console.WriteLine("{0,25}", "-=Teams=-");
        Console.WriteLine("{0,-25}{1,-25}", "Teamleads", "Juniors");
        foreach (var t in teams)
            Console.WriteLine("{0,-25}{1,-25}", t.Teamlead.Name, t.Junior.Name);
        Console.WriteLine("{0,25}", "-=Staisfaction Rate=-");
        Console.WriteLine();
        Console.WriteLine("{0,-25}{1:N2}", "Satisfaction rate:", info.StisfactionRate);
        logger.LogDebug("Stopping application");
        lifetime.StopApplication();
        return Task.CompletedTask;
    }

    public Task Average(CancellationToken cancellationToken)
    {
        var avg = repository.OverallAverageScore();
        Console.WriteLine("Average satisfaction rate: {0:N2}", avg);
        logger.LogDebug("Stopping application");
        lifetime.StopApplication();
        return Task.CompletedTask;
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
        // await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.EnsureCreatedAsync(cancellationToken);
        logger.LogTrace("Database created");
        logger.LogDebug("Database ready");
    }
}
