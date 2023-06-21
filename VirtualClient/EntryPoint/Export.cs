using VirtualClient.Forms;

namespace VirtualClient.EntryPoint;

internal static class Export
{
    public static MainWindow MainForm { get; private set; } = new();

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(MainForm);
    }
}