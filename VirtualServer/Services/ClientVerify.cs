using StreamJsonRpc;

namespace VirtualServer.Services;

internal class ClientVerify
{
    private delegate bool VerifyDelegate(string uuid, string token);
    private delegate byte[] GetServerCertificateDelegate(byte[] userPublicKey);
    public static void Verify(JsonRpc rpc)
    {
        rpc.AddLocalRpcMethod("GetServerCertificate", new GetServerCertificateDelegate((userPublicKey) =>
        {
            return new byte[] { };
        }));
        rpc.AddLocalRpcMethod("VerifyPermission", new VerifyDelegate((uuid, token) =>
        {

        }));
    }

}