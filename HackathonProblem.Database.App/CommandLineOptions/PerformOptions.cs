using CommandLine;

namespace HackathonProblem.Database.App.CommandLineOptions;

[Verb("perform", isDefault: true, HelpText = "Perform hackathon with random wishlists and save to database.")]
public class PerformOptions { }
