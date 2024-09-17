namespace HackathonProblem;

public interface ITeam
{
    IEmployee Teamlead { get; }
    IEmployee Junior { get; }
}

public record Team(IEmployee Teamlead, IEmployee Junior) : ITeam;
