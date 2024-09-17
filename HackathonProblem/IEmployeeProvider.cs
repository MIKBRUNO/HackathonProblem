namespace HackathonProblem;

public interface IEmployeeProvider
{
    IEnumerable<IEmployee> GetEmployees();
}
