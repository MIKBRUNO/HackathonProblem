namespace HackathonProblem.HackathonHost;

public class RepeatHackathonWorker(
    ILogger<RepeatHackathonWorker> logger,
    [FromKeyedServices("teamleads-provider")] IEmployeeProvider teamleadsProvider,
    [FromKeyedServices("juniors-provider")] IEmployeeProvider juniorsProvider,
    IHRManager manager, IHRDirector director,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly ILogger<RepeatHackathonWorker> logger = logger;
    private readonly IEmployeeProvider teamleadsProvider = teamleadsProvider;
    private readonly IEmployeeProvider juniorsProvider = juniorsProvider;
    private readonly IHRManager manager = manager;
    private readonly IHRDirector director = director;
    private readonly IServiceScopeFactory scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var juniors = juniorsProvider.GetEmployees();
        var teamleads = teamleadsProvider.GetEmployees();
        logger.LogInformation("Starting hackathons");
        int i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope()) {
                var hackathon = scope.ServiceProvider.GetRequiredService<IHackathon>();
                var result = hackathon.Perform(teamleads, juniors, manager, director);
                logger.LogInformation("{}: {}", i, result.SatisfactionRate);
                ++i;
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
