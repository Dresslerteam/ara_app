//using Ara.Domain.Repositories;
//using Mono.Data.Sqlite;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Scripts.Infrastructure
//{
//    public class SqlLiteDataProvider : IDataProvider
//    {
//        public IDbConnection Db { get; set; }
//        public SqlLiteDataProvider()
//        {
//            this.Db = this.CreateAndOpenDatabase();
//        }

//        private IDbConnection CreateAndOpenDatabase()
//        {
//            // Open a connection to the database.
//            string dbUri = "URI=file:AraDatabase.sqlite";
//            IDbConnection dbConnection = new SqliteConnection(dbUri);
//            dbConnection.Open();

//            // Create a table for the hit count in the database if it does not exist yet.
//            IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
//            dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS TaskPhotos (id UNIQUEIDENTIFIER PRIMARY KEY, taskId INTEGER, createdOn datetime, createdBy INTEGER, IsSynchronized BIT, synchronizedOn)";
//            dbCommandCreateTable.ExecuteReader();

//            return dbConnection;
//        }
//    }
//}
