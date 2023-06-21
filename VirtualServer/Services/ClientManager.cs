using System.Collections.Concurrent;
using System.Net.Sockets;

namespace VirtualServer.Services;

internal class ClientManager
{
    private readonly ConcurrentDictionary<string, TcpClient> clients = new();

    public bool Add(string uuid, TcpClient client)
    {
        return clients.TryAdd(uuid, client);
    }

    public bool Get(string uuid, out TcpClient? client)
    {
        return clients.TryGetValue(uuid, out client);
    }

    public bool Remove(string uuid)
    {
        return clients.TryRemove(uuid, out _);
    }

    public bool Update(string uuid, TcpClient client)
    {
        return Contains(uuid) && clients.TryUpdate(uuid, client, clients[uuid]);
    }

    public bool Contains(string uuid)
    {
        return clients.ContainsKey(uuid);
    }

    public void Clear()
    {
        clients.Clear();
    }
}