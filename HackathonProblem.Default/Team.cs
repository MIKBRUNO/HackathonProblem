namespace HackathonProblem.Default;

public record class Team(IEmployee Teamlead, IEmployee Junior) : ITeam;

public class TeamFactory : ITeamFactory
{
    public ITeam CreateTeam(IEmployee Teamlead, IEmployee Junior) => new Team(Teamlead, Junior);
}
