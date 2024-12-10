namespace HackathonProblem;

public interface IHackathonRepository
{
    IHackathonInfo PerformAndSave(IEnumerable<IEmployee> teamleads, IEnumerable<IEmployee> juniors,
        IHRManager manager, IHRDirector director);

    IHackathonInfo Load(int id);

    double OverallAverageScore();
}
