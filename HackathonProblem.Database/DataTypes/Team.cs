namespace HackathonProblem.Database.DataTypes;

public class Team : ITeam
{
    public int Id { get; set; }
 
    public int HackathonId { get; set; }

    public Teamlead Teamlead { get; set; }

    public Junior Junior { get; set; }

    IEmployee ITeam.Teamlead => Teamlead;

    IEmployee ITeam.Junior => Junior;
}

public delegate ITeam Ass();

public class TeamFactory : ITeamFactory
{
    public ITeam CreateTeam(IEmployee Teamlead, IEmployee Junior) => new Team()
    {
        Teamlead = Teamlead as Teamlead,
        Junior = Junior as Junior
    };
}
