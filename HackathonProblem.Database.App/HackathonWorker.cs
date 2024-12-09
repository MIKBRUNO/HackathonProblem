using HackathonProblem.Database.DataTypes;
using HackathonProblem.Database.EmployeeProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HackathonProblem.Database.App;

public class HackathonWorker(
    ILogger<HackathonWorker> logger, IHostApplicationLifetime lifetime,
    HackathonContext context,
    ITeamleadsProvider teamleadsProvider, IJuniorsProvider juniorsProvider,
    IWishlistGenerator generator
    ) : IHostedService
{
    public async Task Run(CancellationToken cancellationToken)
    {
        var juniors = context.Juniors;
        var teamleads = context.Teamleads;
        var juniorsWishlists = generator.GenerateWishlists(juniors, teamleads)
                .Select(w => w as JuniorWishlist).ToList();
        var teamleadsWishlists = generator.GenerateWishlists(teamleads, juniors)
                .Select(w => w as TeamleadWishlist).ToList();
        await context.Hackathons.AddAsync(new Hackathon() {
            Id = 0,
            JuniorWishlists = juniorsWishlists!,
            TeamleadWishlists = teamleadsWishlists!,
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        lifetime.StopApplication();
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.EnsureCreatedAsync(cancellationToken);
        Task<bool>[] tasks = [
            context.Juniors.AnyAsync(cancellationToken),
            context.Teamleads.AnyAsync(cancellationToken),
        ];
        await Task.WhenAll(tasks);
        if (!tasks.All(t => t.Result))
        {
            await Task.WhenAll([
                context.Juniors.AddRangeAsync(juniorsProvider.GetJuniors(), cancellationToken),
                context.Teamleads.AddRangeAsync(teamleadsProvider.GetTeamleads(), cancellationToken),
            ]);
            await context.SaveChangesAsync(cancellationToken);
        }
        LongRunningTask = Run(cancellationToken);
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await LongRunningTask!;
    }

    private Task? LongRunningTask;
}
