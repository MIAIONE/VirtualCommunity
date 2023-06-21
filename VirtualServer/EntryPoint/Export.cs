using Serilog;
using Serilog.Events;

using VirtualLog;

using VirtualServer.Services;

namespace VirtualServer.EntryPoint;

internal static class Export
{
    private static void Main()
    {
        try
        {
            InitLogger();
        }
        catch
        {
            Console.WriteLine("��־�����ʼ��ʧ��");
            return;
        }

        try
        {
            Log.Information("��������������");

            RunHost();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "����������");
        }
        finally
        {
            Log.Information("���ڹرշ�����");
            Log.CloseAndFlush();
        }
    }

    private static void InitLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Async(async_output =>
            {
                async_output.Console();
                async_output.File(
                    path: LogService.AppLogsDirectory + $"/VirtualServer_{LogService.DateOnlyOfFixed}.log",
                    rollOnFileSizeLimit: true,
                    shared: true
                    );
            })
            .CreateLogger();
    }

    private static void RunHost()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHostedService<Server>();
            })
            .UseSerilog()
            .Build();

        host.Run();
    }
}