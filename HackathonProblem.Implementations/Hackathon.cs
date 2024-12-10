namespace HackathonProblem.Implementations;

public class Hackathon(IWishlistGenerator generator) : IHackathon
{
    private readonly IWishlistGenerator generator = generator;

    public HackathonResult Perform(IEnumerable<IEmployee> teamleads, IEnumerable<IEmployee> juniors, IHRManager manager, IHRDirector director)
    {
        var teamleadsWishlists = generator.GenerateWishlists(teamleads, juniors);
        var juniorsWishlists = generator.GenerateWishlists(juniors, teamleads);
        var teams = manager.BuildTeams(teamleadsWishlists, juniorsWishlists);
        double rate = director.CalculateSatisfaction(teams, teamleadsWishlists, juniorsWishlists);
        return new HackathonResult(teams, rate);
    }
}
