namespace HackathonProblem.Implementations;

public class EmployeeFactory : IEmployeeFactory
{
    public IEmployee CreateEmployee(int Id, string Name)
    {
        return new Employee(Id, Name);
    }
}
