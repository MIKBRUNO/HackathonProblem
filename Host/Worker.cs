namespace HackathonProblem.HackathonHost;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHackathon hackathon;
    private readonly IEmployeeProvider teamleadsProvider;
    private readonly IEmployeeProvider juniorsProvider;
    private readonly IHRManager manager;
    private readonly IHRDirector director;

    public Worker(
        ILogger<Worker> logger,
        IHackathon hackathon,
        [FromKeyedServices("teamleads-provider")] IEmployeeProvider teamleadsProvider,
        [FromKeyedServices("juniors-provider")] IEmployeeProvider juniorsProvider,
        IHRManager manager, IHRDirector director)
    {
        _logger = logger;
        this.hackathon = hackathon;
        this.teamleadsProvider = teamleadsProvider;
        this.juniorsProvider = juniorsProvider;
        this.manager = manager;
        this.director = director;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var juniors = juniorsProvider.GetEmployees();
        var teamleads = teamleadsProvider.GetEmployees();
        _logger.LogInformation("Starting hackathons");
        int i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = hackathon.Perform(teamleads, juniors, manager, director);
            _logger.LogInformation("{}: {}", i, result.SatisfactionRate);
            ++i;
            await Task.Delay(1000, stoppingToken);
        }
    }
}
