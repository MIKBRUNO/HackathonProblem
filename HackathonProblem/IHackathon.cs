namespace HackathonProblem;

public record HackathonResult(IEnumerable<ITeam> Teams, double SatisfactionRate);

public interface IHackathon
{
    HackathonResult Perform(IEnumerable<IEmployee> teamleads, IEnumerable<IEmployee> juniors, IHRManager manager, IHRDirector director);
}
