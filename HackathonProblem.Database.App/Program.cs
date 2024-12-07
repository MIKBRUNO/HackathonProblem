using HackathonProblem;
using HackathonProblem.Database;
using HackathonProblem.Database.App;
using HackathonProblem.Database.EmployeeProviders;
using HackathonProblem.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<HackathonWorker>();
        
        services.AddSingleton<Random>();
        services.AddTransient<IWishlistGenerator, RandomWishlistGenerator>();
        
        services.AddTransient<IEmployeeFactory, EmployeeFactory>();
        services.AddTransient<IJuniorsProvider, CSVJuniorsProvider>();
        services.AddTransient<ITeamleadsProvider, CSVTeamleadsProvider>();
        
        services.AddDbContext<HackathonContext>();
        
        services.Configure<CSVEmployeeProviderOptions>(
            context.Configuration.GetSection(nameof(CSVEmployeeProviderOptions))
        );
    });

var host = builder.Build();
host.Run();
