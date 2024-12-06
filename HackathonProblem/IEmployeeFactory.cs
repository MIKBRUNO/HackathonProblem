namespace HackathonProblem;

public interface IEmployeeFactory
{
    IEmployee CreateEmployee(int Id, string Name);
}
