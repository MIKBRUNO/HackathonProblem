namespace HackathonProblem;

public interface IHRManager
{
    IEnumerable<ITeam> BuildTeams(
        IEnumerable<IWishlist> teamleadsWishlists, IEnumerable<IWishlist> juniorsWishlists);
}
