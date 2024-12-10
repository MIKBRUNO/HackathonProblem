namespace HackathonProblem.Database.DataTypes;

public class Hackathon
{
    public int Id { get; set; }

    public List<Teamlead> Teamleads { get; set; }

    public List<Junior> Juniors { get; set; }

    public List<TeamleadWishlist> TeamleadWishlists { get; set; }

    public List<JuniorWishlist> JuniorWishlists { get; set; }

    public List<Team> Teams { get; set; }

    public double StisfactionRate { get; set; }
}
