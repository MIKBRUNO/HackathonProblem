using CommandLine;

namespace HackathonProblem.Database.App.CommandLineOptions;

[Verb("show", HelpText = "Shows information about hackathon by its Id.")]
public class ShowOptions
{
    [Option("employees", HelpText = "Show employees list.", Default = false)]
    public bool Employees { get; set; }

    [Option("wishlists", HelpText = "Show wishlists.", Default = false)]
    public bool Wishlists { get; set; }

    [Value(0, HelpText = "Hackathon Id", Required = true, MetaName = "id")]
    public int Id { get; set; }
}
