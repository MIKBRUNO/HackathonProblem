using HackathonProblem.Base;
using HackathonProblem.Utils;

IHackathon hackathon = new HackathonRandom();
// proper way to do resources?
IEnumerable<Employee> teamleads = Utils.ReadEmployeesFromCSV("Resources/Teamleads20.csv");
IEnumerable<Employee> juniors = Utils.ReadEmployeesFromCSV("Resources/Juniors20.csv");
IEnumerable<Wishlist> teamleadsWishlists = new List<Wishlist>(20);
IEnumerable<Wishlist> juniorsWishlists = new List<Wishlist>(20);
hackathon.PerformHackathon(teamleads, juniors, out teamleadsWishlists, out juniorsWishlists);
