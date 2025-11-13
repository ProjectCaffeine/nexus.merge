using Terminal.Gui;

public class MainWindow : Window
{
  //public static string Directory;

  public MainWindow()
  {
    Title = $"Nexus.Merge ({Application.QuitKey} to quit)";

    var directoryLabel = new Label { Text = "Working Directory:" };
    var directoryInput = new FileDialog
    {
      X = Pos.Right(directoryLabel) + 1,
      Width = Dim.Fill()
    };

    Add(directoryLabel, directoryInput);
  }
}
