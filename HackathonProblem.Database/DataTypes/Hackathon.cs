namespace HackathonProblem.Database.DataTypes;

public class Hackathon
{
    public int Id { get; set; }

    public List<TeamleadWishlist> TeamleadWishlists { get; set; }

    public List<JuniorWishlist> JuniorWishlists { get; set; }
}
