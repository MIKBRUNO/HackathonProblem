namespace HackathonProblem;

public interface IEmployee
{
    public int Id { get; }
    public string Name { get; }
}

public record class Employee(int Id, string Name) : IEmployee;
