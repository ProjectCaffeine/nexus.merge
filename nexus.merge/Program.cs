
using System.Diagnostics;

string command = "status";

using (Process process = new())
{
  process.StartInfo.WorkingDirectory = "/home/omfarm/Repos/NexusMergeProvingGrounds";
  process.StartInfo.FileName = "git";
  process.StartInfo.Arguments = command;
  process.StartInfo.UseShellExecute = false;
  process.StartInfo.RedirectStandardError = true;
  process.StartInfo.RedirectStandardOutput = true;
  process.StartInfo.CreateNoWindow = true;

  process.Start();

  string output = await process.StandardOutput.ReadToEndAsync();
  string error = await process.StandardError.ReadToEndAsync();

  process.WaitForExit();

  Console.WriteLine("--- Command Output ---");
  Console.WriteLine(output);
  Console.WriteLine("--- Command Error ---");
  Console.WriteLine(error);
  Console.WriteLine($"Exit code: {process.ExitCode}");
}
