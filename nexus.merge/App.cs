namespace nexus.merge
{
  public class App
  {
    private readonly MergeHelper _mergeHelper;

    public App(MergeHelper mergeHelper)
    {
      _mergeHelper = mergeHelper;
    }

    public async Task Run()
    {
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

      await _mergeHelper.InitMergeProcess(project, repoId);
    }
  }
}
