using HackathonProblem.Database.DataTypes;

namespace HackathonProblem.Database.EmployeeProviders;

public interface IJuniorsProvider : IEmployeeProvider
{
    IEnumerable<Junior> GetJuniors();
}
