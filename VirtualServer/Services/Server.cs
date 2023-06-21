using System.Net.Quic;
using System.Text;

using Serilog;

using StreamJsonRpc;

using VirtualServer.Settings;

namespace VirtualServer.Services;

internal class Server : BackgroundService
{
    private readonly Base listner = new();
    private readonly ClientManager clients = new();
    private readonly LocalDatabase database = new();

    public Server()
    {

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Log.Information("读取配置文件");
            Setting.Default = Setting.Read();
            var infoOutput = new StringBuilder();
            infoOutput.AppendLine("配置文件准备就绪");
            infoOutput.AppendLine($"Port: {Setting.Default?.Port}");
            Log.Information(infoOutput.ToString());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "读取配置文件失败");
        }

        try
        {
            listner.Start();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "开启监听器时发生异常");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var tcpclient = await listner.AcceptTcpClientAsync(CancellationToken.None);

            var clientid = Guid.NewGuid().ToString("X");

            if (clients.Add(clientid, tcpclient))
            {
                var stream = tcpclient.GetStream();

                var jsonrpc = new JsonRpc(stream);

                jsonrpc.Disconnected += (s, e) =>
                {
                    var reason = new StringBuilder();

                    reason.AppendLine("客户端断开连接");
                    reason.AppendLine($"原因: {e.Reason}");
                    reason.AppendLine();
                    reason.AppendLine($"异常: {e.Exception}");
                    reason.AppendLine();

                    Log.Information(reason.ToString());

                    try
                    {
                        clients.Get(clientid, out var client);

                        if (client is not null)
                        {
                            if (client.Connected)
                            {
                                client.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "客户端断开连接时发生异常");
                    }
                    finally
                    {
                        clients.Remove(clientid);
                    }
                };

                ClientVerify.Verify(jsonrpc);
            }
        }

        try
        {
            listner.Stop();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "关闭监听器时发生异常");
        }

    }
}