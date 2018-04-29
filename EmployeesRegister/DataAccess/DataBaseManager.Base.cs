using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace EmployeesRegister.DataAccess
{
    partial class DataBaseManager
    {
        public static DataBaseManager Instance
        {
            get
            {
                if (instance is null)
                    instance = new DataBaseManager();
                return instance;
            }
        }
        private static DataBaseManager instance;

        private DataBaseManager()
        {
            InitialiseTables();
        }

        private const string database_name = "data.dblite";
        private readonly SQLiteConnection connection
            = new SQLiteConnection($"Data Source={database_name}; Version=3;");

        private void InitialiseTables()
        {
            var requests = new List<string>();
            requests.Add(@"CREATE TABLE IF NOT EXISTS [organization] (""oid"" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ""name"" TEXT NOT NULL, ""active"" BOOLEAN NOT NULL DEFAULT (1));");
            requests.Add(@"CREATE TABLE IF NOT EXISTS [person] (""pid"" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ""name"" TEXT NOT NULL, ""phone"" TEXT NOT NULL, ""address"" TEXT NOT NULL, ""active"" BOOLEAN NOT NULL DEFAULT (1));");
            requests.Add(@"CREATE TABLE IF NOT EXISTS [employee] (""oid"" INTEGER NOT NULL REFERENCES  [organization](""oid""), ""pid"" INTEGER NOT NULL REFERENCES  [person](""pid""), ""position"" TEXT NOT NULL, ""salary"" REAL NOT NULL, ""active"" BOOLEAN NOT NULL DEFAULT (1));");

            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Join(string.Empty, requests);
                Execute(command);
            }
        }

        private MethodResult<DataTable> Execute(SQLiteCommand command)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var table = new DataTable();
                using (var da = new SQLiteDataAdapter(command))
                {
                    da.Fill(table);
                }

                return new MethodResult<DataTable>(table);
            }
            catch (Exception ex)
            {
                return new MethodResult<DataTable>(null, ex.Message);
            }
        }

        private InsertResult ExecuteNonQuery(SQLiteCommand command)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var id = command.ExecuteNonQuery();
                return new InsertResult(id);
            }
            catch (Exception ex)
            {
                return new InsertResult(ex.Message);
            }
        }
    }
}
