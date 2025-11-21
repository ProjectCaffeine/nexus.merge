using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using nexus.merge.Pocos;

namespace nexus.merge;

public class AdoApiHelper
{
  private readonly AdoSettings _settings;

  public AdoApiHelper(IOptions<AdoSettings> settings)
  {
    _settings = settings.Value;
    Console.WriteLine("Connecting...");

    Connection = new(new Uri("https://dev.azure.com/ProjectCaffeine"), new VssBasicCredential(string.Empty, _settings.Pap));

    Console.WriteLine("Getting Client...");

    Client = Connection.GetClient<GitHttpClient>();
  }

  private GitHttpClient Client { get; set; }

  private VssConnection Connection { get; set; }

  private GitRepository? Repository { get; set; }

  public async Task InitRepo(string project, string repositoryId)
  {
    Repository = await Client.GetRepositoryAsync(project, repositoryId);
  }

  public async Task<List<GitBranchStats>> GetBranchesAsync(string project, string repositoryId)
  {
    return await Client.GetBranchesAsync(project, repositoryId);
  }
}
