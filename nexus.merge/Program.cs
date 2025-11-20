using Microsoft.Extensions.Configuration;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using nexus.merge;
using nexus.merge.Pocos;

IConfiguration config = new ConfigurationBuilder()
  .AddUserSecrets<Program>()
  .AddEnvironmentVariables()
  .Build();

var currentDirectory = Directory.GetCurrentDirectory();

Console.WriteLine($"Current Directory: {currentDirectory}");

var repoRoot = GitHelpers.GetRepoRoot(currentDirectory);

if (repoRoot == null)
{
  Console.WriteLine("You are not currently in a git repository. Please only use this tool while in a git repository.");
  return;
}

var repoUrl = GitHelpers.GetRepoUrl(repoRoot);

if (repoUrl == null)
{
  Console.WriteLine("Could not find repo URL.");
  return;
}

(string project, string repoId) = GitHelpers.GetRepoProjectAndId(repoUrl);

Console.WriteLine($"Repo root: {repoRoot}");
Console.WriteLine($"Repo URL: {repoUrl}");
Console.WriteLine($"Repo Project: {project}");
Console.WriteLine($"Repo ID: {repoId}");

await InitMergeProcess();

async Task InitMergeProcess()
{
  var branches = await AdoTestingGrounds.GetBranchesAsync(config["AdoPap"]!, project, repoId);

  Console.WriteLine("Branches Found:");
  var selectedBranches = SelectBranchesForMerging(branches);
}

HashSet<GitBranchStats> SelectBranchesForMerging(List<GitBranchStats> allBranches)
{

  var selectedBranches = new HashSet<GitBranchStats>();
  var cont = true;

  do
  {
    DisplayBranches(allBranches.Where(ab => !selectedBranches.Any(sb => sb.Name.Equals(ab.Name, StringComparison.CurrentCultureIgnoreCase))).ToList(), true);

    Console.WriteLine(@"Please select the branches you would like to merge using 1 of the following methods:

        - Input a comma separated list of the branch numbers.
        - Input a comma separated list of exact branch names.
        ");

    var input = Console.ReadLine();
    var parseResults = ParseUserInput(input ?? "", allBranches);

    selectedBranches.AddRange(parseResults.FoundBranches);

    Console.WriteLine();
    Console.WriteLine("Selected Branches:");
    DisplayBranches(selectedBranches.ToList(), false);

    if (parseResults.InvalidInputs.Count != 0)
    {
      Console.WriteLine("Invalid selections:");
      Console.WriteLine();
      Console.WriteLine("---------------------");

      foreach (var str in parseResults.InvalidInputs)
      {
        Console.WriteLine($"{str}");
      }

      Console.WriteLine("---------------------");
      Console.WriteLine();
    }

    Console.WriteLine("Would you like to select more branches?\n(Y)es\n(N)o");
    cont = (Console.ReadLine() ?? string.Empty).Equals("y", StringComparison.CurrentCultureIgnoreCase);

  } while (cont);

  return selectedBranches;
}

void DisplayBranches(List<GitBranchStats> branches, bool displayIndeces)
{
  Console.WriteLine();
  Console.WriteLine("---------------------");

  for (var i = 0; i < branches.Count; i++)
  {
    Console.WriteLine($"{(displayIndeces ? $"({i}) " : "")}{branches[i].Name}");
  }

  Console.WriteLine("---------------------");
  Console.WriteLine();
}

BranchInputParseResult ParseUserInput(string input, List<GitBranchStats> allBranches)
{
  var results = new BranchInputParseResult();

  if (string.IsNullOrWhiteSpace(input))
  {
    return results;
  }

  var splitInput = input.Split(",");

  foreach (var str in splitInput)
  {
    var cleanedStr = str.ToLower().Trim();
    var existingBranch = allBranches.FirstOrDefault(b => b.Name.ToLower().Trim() == cleanedStr.ToLower().Trim());

    if (existingBranch != null)
    {
      results.FoundBranches.Add(existingBranch);
      continue;
    }

    if (int.TryParse(cleanedStr, out var result) &&
        result >= 0 &&
        result < allBranches.Count)
    {

      results.FoundBranches.Add(allBranches[result]);
      continue;
    }

    results.InvalidInputs.Add(str);
  }

  return results;
}
