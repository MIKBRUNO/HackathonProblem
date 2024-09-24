namespace HackathonProblem;

public interface IEmployeeFactory
{
    IEmployee createEmployee(int Id, string Name);
}
