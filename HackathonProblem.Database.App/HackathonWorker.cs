using HackathonProblem.Database.EmployeeProviders;
using HackathonProblem.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackathonProblem.Database.App;

public class HackathonWorker(
    ILogger<HackathonWorker> logger, IHostApplicationLifetime lifetime,
    HackathonContext context,
    ITeamleadsProvider teamleadsProvider, IJuniorsProvider juniorsProvider
    ) : IHostedService
{
    public Task Run(CancellationToken cancellationToken)
    {
        var teamleads = context.Teamleads;
        var juniors = context.Juniors;
        logger.LogInformation(teamleads.Count().ToString());
        lifetime.StopApplication();
        return Task.CompletedTask;
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
        if (tasks.All(t => !t.Result))
        {
            await Task.WhenAll([
                context.Juniors.AddRangeAsync(juniorsProvider.GetJuniors(), cancellationToken),
                context.Teamleads.AddRangeAsync(teamleadsProvider.GetTeamleads(), cancellationToken),
            ]);
        }
        LongRunningTask = Run(cancellationToken);
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await LongRunningTask!;
    }

    private Task? LongRunningTask;
}
