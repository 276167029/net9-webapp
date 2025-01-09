using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
namespace LoginDemoApp.SQLite;

public class SQLiteDataAccess
{
    private readonly SqliteConnection _connection;
    public SQLiteDataAccess(SqliteConnection connection)
    {
        _connection = connection;
    }
    public void CreateUserTable()
    {
        _connection.Open();
        string createTableSql = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Email TEXT)";
        using (var command = new SqliteCommand(createTableSql, _connection))
        {
            command.ExecuteNonQuery();
        }
        _connection.Close();
    }

    public void InsertDemo()
    {
        _connection.Open();
        for (int i = 1; i <= 5; i++)
        {
            string name = $"User{i}";
            string email = $"user{i}@example.com";
            string insertSql = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
            using (SqliteCommand command = new SqliteCommand(insertSql, _connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();
            }
        }
        _connection.Close();
    }

    public string GetExecuteScalar()
    {
        _connection.Open();
         string countSql = "SELECT COUNT(*) FROM Users";
          using (SqliteCommand command = new SqliteCommand(countSql, _connection))
            {
                // ExecuteScalar方法返回结果集中第一行第一列的值，在这里就是记录数量
                int recordCount = Convert.ToInt32(command.ExecuteScalar());
                countSql=recordCount.ToString();
                Console.WriteLine($"Users表中共有 {recordCount} 条记录");
            }
        _connection.Close();

        return countSql;
    }
    public void GetDataTable()
    {
     _connection.Open();
            string selectSql = "SELECT * FROM Users";
            using (SqliteCommand command = new SqliteCommand(selectSql, _connection))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    // 创建DataTable
                    DataTable dataTable = new DataTable();
                    // 填充列信息
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                    }
                    // 填充行数据
                    while (reader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.GetValue(i);
                        }
                        dataTable.Rows.Add(row);
                    }
                    // 可以在这里对DataTable进行后续操作，比如遍历输出数据等
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int id = Convert.ToInt32(row["Id"]);
                        string name = row["Name"].ToString();
                        string email = row["Email"].ToString();
                        Console.WriteLine($"Id: {id}, Name: {name}, Email: {email}");
                    }
                }
            }
            _connection.Close();
    }

}
