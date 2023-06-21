using System.Net;
using System.Net.Sockets;

using VirtualServer.Settings;

namespace VirtualServer.Services;

internal class Base : TcpListener
{
    public Base() : base(IPAddress.IPv6Any, Setting.Default.Port)
    {
        Server.DualMode = true;
        Server.EnableBroadcast = true;
        Server.ExclusiveAddressUse = false;
        Server.NoDelay = true;
        Server.DontFragment = false;
        Server.SendTimeout = Setting.Default.SocketTimeout;
        Server.ReceiveTimeout = Setting.Default.SocketTimeout;
        Server.Ttl = Setting.Default.Ttl;
        Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, Setting.Default.ReuseAddress);
    }
}