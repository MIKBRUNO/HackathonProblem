namespace HackathonProblem;

public interface ITeam
{
    IEmployee Teamlead { get; }
    IEmployee Junior { get; }
}
