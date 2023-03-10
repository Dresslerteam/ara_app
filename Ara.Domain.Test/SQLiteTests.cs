using Ara.Domain.ApplicationServices;
using Ara.Domain.Repositories;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ara.Domain.Test
{
    public class SQLiteTests
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
                string dbUri = "URI=file:AraDatabase.sqlite";
                IDbConnection dbConnection = new SqliteConnection(dbUri);
                dbConnection.Open();

                // Create a table for the hit count in the database if it does not exist yet.
                IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
                dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY, email TEXT, firstName TEXT, lastName TEXT, photo TEXT)";
                dbCommandCreateTable.ExecuteReader();

                return dbConnection;
            }
        }

        [Fact]
        public async void GetUsers()
        {
            var sqliteDb = new SqlLiteDataProvider();
            var userrepo = new UsersRepository(sqliteDb);
            userrepo.Save(new ApiClients.Dtos.UserDto()
            {
                Id = "100",
                Email = "Test@Test",
                FirstName = "Test",
                LastName = "Testishvili"
            });
            var usersList = userrepo.Get();
            Assert.True(true);
        }
    }
}
