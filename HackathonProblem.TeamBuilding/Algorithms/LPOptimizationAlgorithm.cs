using Google.OrTools.LinearSolver;

namespace HackathonProblem.TeamBuilding.Algorithms;

public class LPOptimizationAlgorithm<T> : ITeamBuildingAlgorithm<T>
{
    public IEnumerable<Pair<T>> BuildPairs(
        IEnumerable<IPreferences<T>> teamleadsPreferences,
        IEnumerable<IPreferences<T>> juniorsPreferences)
    {
        Solver solver = Solver.CreateSolver("SCIP");
        if (solver is null)
        {
            throw new Exception("Could not create solver");
        }

        int employeeCount = teamleadsPreferences.Count();

        Variable[,] pairs = new Variable[employeeCount, employeeCount];
        for (int i = 0; i < employeeCount; ++i)
        {
            for (int j = 0; j < employeeCount; ++j)
            {
                pairs[i, j] = solver.MakeIntVar(0, 1, $"teamlead_{i}_junior_{j}");
            }
        }

        // rook's graph constraints
        for (int i = 0; i < employeeCount; ++i)
        {
            Constraint constraint = solver.MakeConstraint(1, 1, "");
            for (int j = 0; j < employeeCount; ++j)
            {
                constraint.SetCoefficient(pairs[i, j], 1);
            }
        }
        for (int j = 0; j < employeeCount; ++j)
        {
            Constraint constraint = solver.MakeConstraint(1, 1, "");
            for (int i = 0; i < employeeCount; ++i)
            {
                constraint.SetCoefficient(pairs[i, j], 1);
            }
        }

        Objective objective = solver.Objective();
        {
            int i = 0;
            foreach (var teamleadPreferences in teamleadsPreferences)
            {
                var teamlead = teamleadPreferences.Owner;
                int j = 0;
                foreach (var juniorPreferences in juniorsPreferences)
                {
                    var junior = juniorPreferences.Owner;
                    var weight = teamleadPreferences.GetRating(junior) + juniorPreferences.GetRating(teamlead);
                    objective.SetCoefficient(pairs[i, j], weight);
                    ++j;
                }
                ++i;
            }
        }
        objective.SetMaximization();

        Solver.ResultStatus status = solver.Solve();
        if (status != Solver.ResultStatus.OPTIMAL && status != Solver.ResultStatus.FEASIBLE)
        {
            throw new TeamBuildingNotFoundException("LP solution not found");
        }

        List<T> teamleads = teamleadsPreferences.Select(p => p.Owner).ToList();
        List<T> juniors = juniorsPreferences.Select(p => p.Owner).ToList();
        List<Pair<T>> result = [];
        for (int i = 0; i < employeeCount; ++i)
        {
            for (int j = 0; j < employeeCount; ++j)
            {
                if (pairs[i, j].SolutionValue() > 0.5)
                {
                    result.Add(new Pair<T>(teamleads[i], juniors[j]));
                }
            }
        }

        return result;
    }
}
