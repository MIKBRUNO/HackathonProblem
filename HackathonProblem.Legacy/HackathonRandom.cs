namespace HackathonProblem.Base.Concepts;

public class HackathonRandom : IHackathon
{
    private static Random random = new();
    public void PerformHackathon(in IEnumerable<Employee> teamleads, in IEnumerable<Employee> juniors,
        out IEnumerable<Wishlist> teamleadsWishlists, out IEnumerable<Wishlist> juniorsWishlists)
    {
        Span<int> juniorsIds = juniors.Select(junior => junior.Id).ToArray();
        random.Shuffle(juniorsIds);
        teamleadsWishlists = new List<Wishlist>(teamleads.Count());
        foreach (Employee teamlead in teamleads)
        {
            teamleadsWishlists = teamleadsWishlists.Append(new Wishlist(teamlead.Id, juniorsIds.ToArray()));
            random.Shuffle(juniorsIds);
        }
        
        Span<int> teamleadsIds = teamleads.Select(teamlead => teamlead.Id).ToArray();
        random.Shuffle(teamleadsIds);
        juniorsWishlists = new List<Wishlist>(juniors.Count());
        foreach (Employee junior in juniors)
        {
            juniorsWishlists = juniorsWishlists.Append(new Wishlist(junior.Id, teamleadsIds.ToArray()));
            random.Shuffle(teamleadsIds);
        }
    }
}
