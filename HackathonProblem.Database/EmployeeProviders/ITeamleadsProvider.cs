using HackathonProblem.Database.DataTypes;

namespace HackathonProblem.Database.EmployeeProviders;

public interface ITeamleadsProvider : IEmployeeProvider
{
    IEnumerable<Teamlead> GetTeamleads();
}
