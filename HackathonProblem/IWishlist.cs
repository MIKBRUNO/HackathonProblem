namespace HackathonProblem;

public interface IWishlist
{
    IEmployee Owner { get; }
    IEnumerable<IEmployee> DesiredEmployees { get; }
}
