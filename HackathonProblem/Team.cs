namespace HackathonProblem;

public interface ITeam
{
    IEmployee Teamlead { get; }
    IEmployee Junior { get; }
}

public record class Team(IEmployee Teamlead, IEmployee Junior) : ITeam;
