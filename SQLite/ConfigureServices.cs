using System;
using Microsoft.Data.Sqlite;
namespace LoginDemoApp.SQLite;

public class Config
{
 public static void ConfigureServices(IServiceCollection services)
    {
        // 配置SQLite数据库连接
        string connectionString = "Data Source=SQLite/SQLite.db";
        services.AddSingleton<SqliteConnection>(new SqliteConnection(connectionString));
         services.AddScoped<SQLiteDataAccess>();
    }
}
