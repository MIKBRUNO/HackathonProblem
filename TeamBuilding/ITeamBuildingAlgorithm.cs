namespace HackathonProblem.TeamBuilding;

public interface ITeamBuildingAlgorithm<T>
{
    IEnumerable<Pair<T>> BuildMarriage(
        IEnumerable<IPreferences<T>> teamleadsPreferences,
        IEnumerable<IPreferences<T>> juniorsPreferences);
}
