using HackathonProblem.Database.DataTypes;
using HackathonProblem.Database.EmployeeProviders;
using HackathonProblem.Implementations;
using Microsoft.Extensions.Options;

namespace HackathonProblem.Database;

public class CSVJuniorsProvider(IOptions<CSVEmployeeProviderOptions> options, EmployeeFactory<Junior> factory)
    : CSVEmployeeProvider(options.Value.JuniorsFilepath, factory), IJuniorsProvider
{
    public IEnumerable<Junior> GetJuniors() => GetEmployees().Select(e => e as Junior);
}
