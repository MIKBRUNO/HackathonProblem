using System.Diagnostics.CodeAnalysis;

namespace HackathonProblem;

public interface IEmployee
{
    public int Id { get; }
    public string Name { get; }
}

public class Employee : IEmployee
{
    public Employee() {}
    
    [SetsRequiredMembers]
    public Employee(int Id, string Name) : this() { this.Id = Id; this.Name = Name; }
    
    [SetsRequiredMembers]
    public Employee(IEmployee original) : this(original.Id, original.Name) {}
    
    public required int Id { get; set; }
    
    public required string Name { get; set; }
}
