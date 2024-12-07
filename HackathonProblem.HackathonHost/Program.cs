using HackathonProblem;
using HackathonProblem.Default;
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
        services.AddHostedService<RepeatHackathonWorker>();
        services.AddScoped<IHackathon, Hackathon>();
        services.AddSingleton<Random>();
        services.AddTransient<IWishlistGenerator, RandomWishlistGenerator>();
        services.AddTransient<IHRDirector, HRDirector>();
        services.AddTransient<IHRManager, HRManager>();
        services.AddTransient<ITeamBuildingAlgorithm<IEmployee>, GaleShapleyAlgorithm<IEmployee>>();
        services.AddTransient<ITeamFactory, TeamFactory>();
        services.AddTransient<IWishlistFactory, WishlistFactory>();
        services.AddTransient<IEmployeeFactory, EmployeeFactory>();
        services.AddTransient<IPreferencesFactory<IEmployee>, PreferencesFactory<IEmployee>>();
        services.AddKeyedTransient<IEmployeeProvider, CSVEmployeeProvider>(
            "teamleads-provider", (_, _) => new CSVEmployeeProvider(TeamleadsPath, new EmployeeFactory())
        );
        services.AddKeyedTransient<IEmployeeProvider, CSVEmployeeProvider>(
            "juniors-provider", (_, _) => new CSVEmployeeProvider(JuniorsPath, new EmployeeFactory())
        );
    }
);

var host = builder.Build();
host.Run();
