using LibGit2Sharp;

namespace nexus.merge
{
  public static class GitHelpers
  {
    public static string? GetRepoRoot(string directory)
    {
      var inRepo = Repository.IsValid(directory);

      while (!inRepo)
      {
        var parent = Directory.GetParent(directory);

        if (parent == null)
        {
          return null;
        }

        directory = parent.FullName;
        inRepo = Repository.IsValid(directory);
      }

      return directory;
    }

    public static string? GetRepoUrl(string repoRoot)
    {
      using (var repo = new Repository(repoRoot))
      {
        var remote = repo.Network.Remotes["origin"];

        if (remote != null)
        {
          return remote.Url;
        }

        return null;
      }
    }

    public static (string project, string repoId) GetRepoProjectAndId(string repoUrl)
    {
      var splitUrl = repoUrl.Split("/");

      return (splitUrl[^2], splitUrl[^1]);
    }
  }
}
