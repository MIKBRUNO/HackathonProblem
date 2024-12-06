using HackathonProblem.Database.DataTypes;
using HackathonProblem.Database.EmployeeProviders;
using HackathonProblem.Implementations;
using Microsoft.Extensions.Options;

namespace HackathonProblem.Database;

public class CSVJuniorsProvider(IOptions<CSVEmployeeProviderOptions> options, IEmployeeFactory factory)
    : CSVEmployeeProvider(options.Value.JuniorsFilepath, factory), IJuniorsProvider
{
    public IEnumerable<Junior> GetJuniors()
    {
        return GetEmployees().Select(e => new Junior(e));
    }
}
