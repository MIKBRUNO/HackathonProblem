namespace HackathonProblem.Implementations;

public static class HarmonicMeanEnumerableExtension
{
    public static double HarmonicMean(this IEnumerable<double>? doubles)
    {
        double acc = 0;
        if (doubles is null || !doubles.Any()) return 0;
        foreach(var d in doubles)
        {
            acc += 1.0 / d;
        }
        return doubles.Count() / acc;
    }
}
