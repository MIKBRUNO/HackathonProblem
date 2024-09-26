namespace HackathonProblem.TeamBuilding;

public class Preferences<T> : IPreferences<T> where T : notnull
{
    public T Owner { get; init; }
    
    private readonly IEnumerable<T> orderedPreferences;

    private readonly Dictionary<T, int> ratings = [];

    public Preferences(T Owner, IEnumerable<T> orderedPreferences)
    {
        this.Owner = Owner;
        this.orderedPreferences = orderedPreferences;
        int i = orderedPreferences.Count();
        foreach (var pref in orderedPreferences)
        {
            ratings[pref] = i;
            --i;
        }
    }

    public T Compare(T left, T right)
    {
        return ratings[left] > ratings[right] ? left : right;
    }

    public IEnumerable<T> Enumerate()
    {
        return orderedPreferences;
    }

    public int GetRating(T preferrable)
    {
        return ratings[preferrable];
    }
}
