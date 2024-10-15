namespace HackathonProblem.TeamBuilding;

public interface IPreferencesFactory<T>
{
    IPreferences<T> CreatePreferences(T owner, IEnumerable<T> orderedPreferences);
}
