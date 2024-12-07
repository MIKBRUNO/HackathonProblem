using HackathonProblem.Database.DataTypes;
using HackathonProblem.Database.EmployeeProviders;
using Microsoft.EntityFrameworkCore;
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
        var teamleads = context.Teamleads;
        var juniors = context.Juniors;
        // var teamleadsWishlists = generator.GenerateWishlists(teamleads, juniors);
        // var juniorsWishlists = generator.GenerateWishlists(juniors, teamleads);
        // var aa = juniorsWishlists.Select(w => new JuniorWishlist(w)).ToList();
        // context.JuniorWishlists.Add(aa[0]);
        // await context.Hackathons.AddAsync(new Hackathon() {
        //     Id = 0,
        //     JuniorWishlists = juniorsWishlists.Select(w => new JuniorWishlist(w)).ToList(),
        //     TeamleadWishlists = teamleadsWishlists.Select(w => new TeamleadWishlist(w)).ToList(),
        // }, cancellationToken);
        // logger.LogDebug("Finished1");
        // await context.SaveChangesAsync(cancellationToken);
        // logger.LogDebug("Finished2");
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
        logger.LogDebug("Started");
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await LongRunningTask!;
    }

    private Task? LongRunningTask;
}
