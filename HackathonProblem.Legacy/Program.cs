using HackathonProblem.Base.Concepts;
using HackathonProblem.Base.Utils;
using HackathonProblem.Base.TeamBuilding;
using HackathonProblem.Base;

const int ITERATIONS = 1000;

IHackathon hackathon = new HackathonRandom();
// proper way to do resources?
IEnumerable<Employee> teamleads = Utils.ReadEmployeesFromCSV("Resources/Teamleads20.csv");
IEnumerable<Employee> juniors = Utils.ReadEmployeesFromCSV("Resources/Juniors20.csv");
IEnumerable<Wishlist> teamleadsWishlists;
IEnumerable<Wishlist> juniorsWishlists;
var strategy = new GaleShapleyTeamBuildingStrategy();
var calc = new SatisfactionCalculatorHarmonicMean();

Console.WriteLine("Starting {0} hackathons", ITERATIONS);
double sumRatings = 0;
for (int i = 0; i < ITERATIONS; ++i) {
    hackathon.PerformHackathon(teamleads, juniors, out teamleadsWishlists, out juniorsWishlists);
    IEnumerable<Team> teams = strategy.BuildTeams(teamleads, juniors, teamleadsWishlists, juniorsWishlists);
    double satisfactionRating = calc.CalculateSatisfaction(teams, teamleadsWishlists, juniorsWishlists);
    sumRatings += satisfactionRating;
    Console.WriteLine("{0}: {1}", i, satisfactionRating);
}
Console.WriteLine("\nOverall mean: {0}", sumRatings / ITERATIONS);
