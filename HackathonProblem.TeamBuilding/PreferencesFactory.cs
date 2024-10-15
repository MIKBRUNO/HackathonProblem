
namespace HackathonProblem.TeamBuilding;

public class PreferencesFactory<T> : IPreferencesFactory<T> where T : notnull
{
    public IPreferences<T> CreatePreferences(T owner, IEnumerable<T> orderedPreferences) =>
        new Preferences<T>(owner, orderedPreferences);
}
