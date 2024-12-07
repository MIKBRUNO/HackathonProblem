namespace HackathonProblem;

public interface ITeamFactory
{
    ITeam CreateTeam(IEmployee Teamlead, IEmployee Junior);
}
