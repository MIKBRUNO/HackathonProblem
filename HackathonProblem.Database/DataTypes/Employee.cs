namespace HackathonProblem.Database.DataTypes;

public class Employee : IEmployee
{
    public int HackathonId { get; set; }
 
    public int Id { get; set; }

    public string Name { get; set; }
}

public class Junior : Employee;

public class Teamlead : Employee;

public class EmployeeFactory<T> : IEmployeeFactory
    where T : Employee, new()
{
    public IEmployee CreateEmployee(int Id, string Name) => new T() { Id = Id, Name = Name };
}
