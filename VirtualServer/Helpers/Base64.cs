using System.Text;

namespace VirtualServer.Helpers;

internal static class Base64
{
    public static string Encode(this string text)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }

    public static string Decode(string base64)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    }
}