
//using Mono.Data.Sqlite;
using System.Data;

using Ara.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class SqlLiteDataProvider : IDataProvider
    {
        public IDbConnection Db { get; set; }
        public SqlLiteDataProvider()
        {
            this.Db = this.CreateAndOpenDatabase();
        }

        private IDbConnection CreateAndOpenDatabase()
        {
            // Open a connection to the database.
            string dbUri = $"URI=file:{Application.persistentDataPath}/AraDatabase.sqlite";
            IDbConnection dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            // Create a table for the hit count in the database if it does not exist yet.
            IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
            dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY, email TEXT, firstName TEXT, lastName TEXT, photo TEXT)";
            dbCommandCreateTable.ExecuteReader();

            return dbConnection;
        }
    }
}
