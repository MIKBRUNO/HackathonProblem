using HackathonProblem;
using HackathonProblem.HackathonHost;
using HackathonProblem.Implementations;
using HackathonProblem.TeamBuilding;
using HackathonProblem.TeamBuilding.Algorithms;

const string JuniorsPath = "Resources/Juniors20.csv";
const string TeamleadsPath = "Resources/Teamleads20.csv";

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(
    (_, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddTransient<IHackathon, Hackathon>();
        services.AddTransient<IWishlistGenerator>(_ => new RandomWishlistGenerator(new Random()));
        services.AddTransient<IHRDirector, HRDirector>();
        services.AddTransient<IHRManager, HRManager>();
        services.AddTransient<IMarriageAlgorithm<IEmployee>, GaleShapleyAlgorithm<IEmployee>>();
        services.AddKeyedSingleton<IEmployeeProvider, CSVEmployeeProvider>(
            "teamleads-provider", (_, _) => new CSVEmployeeProvider(TeamleadsPath, new EmployeeFactory())
        );
        services.AddKeyedSingleton<IEmployeeProvider, CSVEmployeeProvider>(
            "juniors-provider", (_, _) => new CSVEmployeeProvider(JuniorsPath, new EmployeeFactory())
        );
    }
);

var host = builder.Build();
host.Run();
