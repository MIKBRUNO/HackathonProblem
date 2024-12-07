namespace HackathonProblem.Default;

public record class Employee(int Id, string Name) : IEmployee;

public class EmployeeFactory : IEmployeeFactory
{
    public IEmployee CreateEmployee(int Id, string Name) => new Employee(Id, Name);
}
