namespace HackathonProblem;

public interface IHackathonInfo
{
    int Id { get; }

    IEnumerable<IEmployee> Teamleads { get; }

    IEnumerable<IEmployee> Juniors { get; }

    IEnumerable<IWishlist> TeamleadWishlists { get; }

    IEnumerable<IWishlist> JuniorWishlists { get; }

    IEnumerable<ITeam> Teams { get; }

    double StisfactionRate { get; }
}
