using HackathonProblem;
using HackathonProblem.Implementations;
using TeamBuilding.Algorithms;


public static class Program
{
    static readonly int DEFAULT_REPEATS = 1000;
    static readonly string JUNIORS_PATH = "Resources/Juniors20.csv";
    static readonly string TEMALEADS_PATH = "Resources/Teamleads20.csv";
    
    static void Main(string[] args)
    {
        int repeats = DEFAULT_REPEATS;
        if (args.Length > 0)
        {
            repeats = int.Parse(args[0]);
        }
        IHackathon hackathon = new Hackathon(new RandomWishlistGenerator(new Random()));
        IHRManager manager = new HRManager(new GaleShapleyAlgorithm<IEmployee>());
        IHRDirector director = new HRDirector();
        IEnumerable<IEmployee> juniors = new CSVEmployeeProvider(JUNIORS_PATH).GetEmployees();
        IEnumerable<IEmployee> teamleads = new CSVEmployeeProvider(TEMALEADS_PATH).GetEmployees();
        
        Console.WriteLine("Starting {0} hackathons", repeats);
        double sum = 0;
        for (int i = 0; i < repeats; ++i)
        {
            var result = hackathon.Perform(teamleads, juniors, manager, director);
            sum += result.SatisfactionRate;
            Console.WriteLine("{0}: {1}", i, result.SatisfactionRate);
        }
        Console.WriteLine("\nOverall mean: {0}", sum / repeats);
    }
}


