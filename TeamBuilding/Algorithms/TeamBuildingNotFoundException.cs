namespace HackathonProblem.TeamBuilding.Algorithms;

public class TeamBuildingNotFoundException : Exception
{
    public TeamBuildingNotFoundException() : base() {  }
    public TeamBuildingNotFoundException(string? message) : base(message) {  }
    public TeamBuildingNotFoundException(string? message, Exception? inner) : base(message, inner) {  }
}
