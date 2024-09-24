namespace HackathonProblem.Implementations;

public class EmployeeFactory : IEmployeeFactory
{
    public IEmployee createEmployee(int Id, string Name)
    {
        return new Employee(Id, Name);
    }
}
