using System.Text;
using System.Text.Json;

namespace VirtualServer.Settings;

internal class Setting
{
    public static string AppDirectory => Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
    public static string AppSettingsFile => Path.Combine(AppDirectory, "ServerSettings.json");
    public static string AppDatabaseFile => Path.Combine(AppDirectory, "ServerData.db");
    public static Data Default { get; set; } = new();

    public static readonly JsonSerializerOptions JsonOption = new()
    {
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = true,
        WriteIndented = true
    };

    public static T Deserialize<T>(string value) where T : class
    {
        return JsonSerializer.Deserialize<T>(value, JsonOption) ?? throw new ApplicationException();
    }

    public static string Serialize<T>(T value) where T : class
    {
        return JsonSerializer.Serialize(value, JsonOption);
    }

    public static Data Read(string filepath)
    {
        return Deserialize<Data>(File.ReadAllText(filepath, Encoding.UTF8));
    }

    public static Data Read()
    {
        return Read(AppSettingsFile);
    }

    public static void Write(Data setting, string filepath)
    {
        File.WriteAllText(filepath, Serialize(setting), Encoding.UTF8);
    }

    public static void Write(string filepath)
    {
        Write(Default, filepath);
    }

    public static void Write()
    {
        Write(Default, AppSettingsFile);
    }

    public static void Load()
    {
        Default = Read();
    }

    public static Data Reload()
    {
        Load();
        return Default;
    }

    public static void Save()
    {
        Write();
    }

    public static void SaveAndReload()
    {
        Save();
        Reload();
    }

    public class Data
    {
        public int Port { get; set; }
        public int SocketTimeout { get; set; }
        public short Ttl { get; set; }
        public bool ReuseAddress { get; set; }
        public SecuitySettings Secuity { get; set; } = new();

        public class SecuitySettings
        {
            public bool UseDifferentKeyForEachLifetime { get; set; }
            public int Lifetime { get; set; }
            public int KeySize { get; set; }
        }
    }
}