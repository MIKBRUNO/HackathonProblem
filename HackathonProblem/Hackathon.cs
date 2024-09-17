namespace HackathonProblem;

public class Hackathon : IHackathon
{
    private readonly IWishlistGenerator TeamleadsWishlistGenerator;
    private readonly IWishlistGenerator JuniorsWishlistGenerator;

    public Hackathon(IWishlistGenerator teamleadsGenerator, IWishlistGenerator juniorsGenerator)
    {
        TeamleadsWishlistGenerator = teamleadsGenerator;
        JuniorsWishlistGenerator = juniorsGenerator;
    }

    public Hackathon(IWishlistGenerator generalWishlistGenerator)
    {
        TeamleadsWishlistGenerator = generalWishlistGenerator;
        JuniorsWishlistGenerator = generalWishlistGenerator;
    }

    public HackathonResult Perform(IEnumerable<IEmployee> teamleads, IEnumerable<IEmployee> juniors, IHRManager manager, IHRDirector director)
    {
        IEnumerable<IWishlist> teamleadsWishlists = TeamleadsWishlistGenerator.GenerateWishlists(teamleads, juniors);
        IEnumerable<IWishlist> juniorsWishlists = JuniorsWishlistGenerator.GenerateWishlists(juniors, teamleads);
        IEnumerable<ITeam> teams = manager.BuildTeams(teamleadsWishlists, juniorsWishlists);
        double rate = director.CalculateSatisfaction(teams, teamleadsWishlists, juniorsWishlists);
        return new HackathonResult(teams, rate);
    }
}
