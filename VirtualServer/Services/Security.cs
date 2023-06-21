using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Serilog;

using VirtualServer.Settings;

namespace VirtualServer.Services;

internal class Security
{
    public static RSAParameters CreateKey()
    {
        Log.Information($"正在重新生成密钥对");
        using var rsaProvider = RSA.Create(Setting.Default.Secuity.KeySize);

        Log.Information("生成密钥对成功");
        return rsaProvider.ExportParameters(true);
    }

    public static RSA GetProviderByKey(RSAParameters keyPair)
    {
        Log.Information($"正在导入密钥对");
        var rsaProvider = RSA.Create(Setting.Default.Secuity.KeySize);
        rsaProvider.ImportParameters(keyPair);
        Log.Information("生成密钥容器对象成功");
        return rsaProvider;
    }

    //严禁传入私钥, 仅允许传入公钥, 否则等同明文
    public static X509Certificate2 CreateCertificate(RSAParameters keyPair, bool isrootCa = true)
    {
        Log.Information("正在获取密钥对");

        var rsa = GetProviderByKey(keyPair);

        Log.Information("正在生成X509证书");

        var request = new CertificateRequest(
            $"CN=VirtualSign,OU=VirtualServer,O=TrustNetwork,serialNumber={Guid.NewGuid():X}",
            rsa,
            HashAlgorithmName.SHA512,
            RSASignaturePadding.Pkcs1
            );

        var basicConstraints = new X509BasicConstraintsExtension(
            isrootCa, false, 0, true
            );

        request.CertificateExtensions.Add(basicConstraints);

        var keyUsage = new X509KeyUsageExtension(
            X509KeyUsageFlags.DigitalSignature |
            X509KeyUsageFlags.KeyEncipherment |
            X509KeyUsageFlags.DataEncipherment |
            X509KeyUsageFlags.KeyAgreement |
            X509KeyUsageFlags.KeyCertSign |
            X509KeyUsageFlags.CrlSign
            , true);

        request.CertificateExtensions.Add(keyUsage);

        request.CertificateExtensions.Add(new X509AuthorityKeyIdentifierExtension());
        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension());

        var certificate = request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(31)
            );

        Log.Information("证书生成成功");

        return certificate;
    }
}