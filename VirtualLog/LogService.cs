namespace VirtualLog;

public static class LogService
{
    public static string DateOnlyOfFixed =>
        DateOnly.FromDateTime(DateTime.Now).ToString().Replace("/", "_");

    public static string AppDirectory => Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
    public static string AppLogsDirectory => Path.Combine(AppDirectory, "logs");
}