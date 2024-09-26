namespace HackathonProblem.TeamBuilding;

public interface IMarriageAlgorithm<T>
{
    IEnumerable<Pair<T>> BuildMarriage(
        IEnumerable<IPreferences<T>> menPreferences,
        IEnumerable<IPreferences<T>> womenPreferences);
}
