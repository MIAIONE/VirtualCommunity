using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace VirtualServer.Models;

internal class SecurityExchangeModel
{
    public string? UUID { get; set; }
    //每个客户端的服务端密码单独协商, 服务端临时保存, 客户端主动发送
    public byte[]? Client_Password { get; set; }
    public byte[]? Client_IV { get; set; }

}