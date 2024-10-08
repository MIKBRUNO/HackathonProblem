using HackathonProblem.TeamBuilding;

namespace HackathonProblem.Implementations.Test;

public class HRManagerTests
{
    private class CallCountTeamBuildingAlgorithm : ITeamBuildingAlgorithm<IEmployee>
    {
        public int CallCount { get; private set; }

        public IEnumerable<Pair<IEmployee>> BuildPairs(
            IEnumerable<IPreferences<IEmployee>> teamleadsPreferences,
            IEnumerable<IPreferences<IEmployee>> juniorsPreferences)
        {
            CallCount += 1;
            return [];
        }
    }

    [Fact]
    public void BuildTeams_StrategyMusctBeCalledOnce()
    {
        var callCount = new CallCountTeamBuildingAlgorithm();
        var manager = new HRManager(callCount);
        manager.BuildTeams([], []);
        Assert.Equal(1, callCount.CallCount);
    }
}
