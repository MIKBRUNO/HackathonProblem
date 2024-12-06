using System.Diagnostics.CodeAnalysis;

namespace HackathonProblem.Database.DataTypes;

public class Teamlead : Employee
{
    public Teamlead() : base() {}

    [SetsRequiredMembers]
    public Teamlead(int Id, string Name) : base(Id, Name) {}

    [SetsRequiredMembers]
    public Teamlead(IEmployee employee) : base(employee) {}
}
public class Junior : Employee
{
    public Junior() : base() {}

    [SetsRequiredMembers]
    public Junior(int Id, string Name) : base(Id, Name) {}

    [SetsRequiredMembers]
    public Junior(IEmployee employee) : base(employee) {}
}