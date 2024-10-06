namespace HackathonProblem.TeamBuilding;

public interface ITeamBuildingAlgorithm<T>
{
    IEnumerable<Pair<T>> BuildPairs(
        IEnumerable<IPreferences<T>> teamleadsPreferences,
        IEnumerable<IPreferences<T>> juniorsPreferences);
}
