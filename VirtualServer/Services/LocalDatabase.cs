

using LiteDB;

using VirtualServer.Models;
using VirtualServer.Settings;

namespace VirtualServer.Services;

internal class LocalDatabase
{
    private readonly LiteDatabase database;
    private readonly ILiteCollection<SecurityExchangeModel> SEM;
    public LocalDatabase()
    {
        database = new LiteDatabase(Setting.AppDatabaseFile);
        SEM = database.GetCollection<SecurityExchangeModel>();
      
    }
}