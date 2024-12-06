using HackathonProblem.Database.DataTypes;
using HackathonProblem.Database.EmployeeProviders;
using HackathonProblem.Implementations;
using Microsoft.Extensions.Options;

namespace HackathonProblem.Database;

public class CSVTeamleadsProvider(IOptions<CSVEmployeeProviderOptions> options, IEmployeeFactory factory)
    : CSVEmployeeProvider(options.Value.TeamleadsFilepath, factory), ITeamleadsProvider
{
    public IEnumerable<Teamlead> GetTeamleads()
    {
        return GetEmployees().Select(e => new Teamlead(e));
    }
}
