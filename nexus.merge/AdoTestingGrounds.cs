using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace nexus.merge;

public static class AdoTestingGrounds
{
  public static async Task<List<GitBranchStats>> GetBranchesAsync(string pat)
  {
    Console.WriteLine("Connecting.");

    VssConnection connection = new(new Uri("https://dev.azure.com/ProjectCaffeine"), new VssBasicCredential(string.Empty, pat));

    Console.WriteLine("Getting Client");
    var project = "NexusMergeProvingGrounds";
    var repositoryId = "NexusMergeProvingGrounds";

    GitHttpClient gitClient = connection.GetClient<GitHttpClient>();
    GitRepository repository = await gitClient.GetRepositoryAsync(project, repositoryId);

    Console.WriteLine($"default branch: {repository.DefaultBranch}");

    return await gitClient.GetBranchesAsync(project, repositoryId);
  }
}
