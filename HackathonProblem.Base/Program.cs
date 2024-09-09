using HackathonProblem.Base.Concepts;
using HackathonProblem.Base.Utils;
using HackathonProblem.Base.TeamBuilding;
using System.Runtime.InteropServices.Marshalling;

IHackathon hackathon = new HackathonRandom();
// proper way to do resources?
IEnumerable<Employee> teamleads = Utils.ReadEmployeesFromCSV("Resources/Teamleads5.csv");
IEnumerable<Employee> juniors = Utils.ReadEmployeesFromCSV("Resources/Juniors5.csv");
IEnumerable<Wishlist> teamleadsWishlists = new List<Wishlist>(20);
IEnumerable<Wishlist> juniorsWishlists = new List<Wishlist>(20);
hackathon.PerformHackathon(teamleads, juniors, out teamleadsWishlists, out juniorsWishlists);
var strategy = new GaleShapleyTeamBuildingStrategy();
strategy.BuildTeams(teamleads, juniors, teamleadsWishlists, juniorsWishlists);
