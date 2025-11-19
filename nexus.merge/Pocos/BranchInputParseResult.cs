using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace nexus.merge.Pocos
{
  public class BranchInputParseResult
  {
    public List<GitBranchStats> FoundBranches { get; set; } = [];

    public List<string> InvalidInputs { get; set; } = [];
  }
}
