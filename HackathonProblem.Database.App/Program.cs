using HackathonProblem;
using HackathonProblem.Database;
using HackathonProblem.Database.App;
using HackathonProblem.Database.DataTypes;
using HackathonProblem.Database.EmployeeProviders;
using HackathonProblem.Implementations;
using HackathonProblem.TeamBuilding;
using HackathonProblem.TeamBuilding.Algorithms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommandLine;
using HackathonProblem.Database.App.CommandLineOptions;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<ParserResult<object>>(s =>
            Parser.Default.ParseArguments<AverageOptions, ShowOptions, PerformOptions>(args)
        );
        services.AddHostedService<HackathonWorker>();
        
        services.AddSingleton<Random>();
        services.AddTransient<IWishlistFactory, WishlistFactory>(); 
        services.AddTransient<IWishlistGenerator, RandomWishlistGenerator>();
        
        services.AddTransient<EmployeeFactory<Junior>>();
        services.AddTransient<EmployeeFactory<Teamlead>>();
        services.AddTransient<IJuniorsProvider, CSVJuniorsProvider>();
        services.AddTransient<ITeamleadsProvider, CSVTeamleadsProvider>();

        services.AddTransient<IHackathonRepository, HackathonRepository>();
        services.AddTransient<IHRDirector, HRDirector>();
        services.AddTransient<IHRManager, HRManager>();
        services.AddTransient<IPreferencesFactory<IEmployee>, PreferencesFactory<IEmployee>>();
        services.AddTransient<ITeamBuildingAlgorithm<IEmployee>, LPOptimizationAlgorithm<IEmployee>>();
        services.AddTransient<ITeamFactory, TeamFactory>();
        
        services.AddDbContext<HackathonContext>(o => o.UseSqlite("name=ConnectionStrings:Database"));
        
        services.Configure<CSVEmployeeProviderOptions>(
            context.Configuration.GetSection(nameof(CSVEmployeeProviderOptions))
        );
    });

var host = builder.Build();
host.Run();
