using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ara.Domain.Repositories
{
    public class UsersRepository
    {
        private readonly IDataProvider _dataProvider;
        public UsersRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<UserDto> Get()
        {
            var list = new List<UserDto>();
            IDbConnection dbConnection = _dataProvider.Db;
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
            dbCommandReadValues.CommandText = "SELECT * FROM Users";
            IDataReader dataReader = dbCommandReadValues.ExecuteReader();

            while (dataReader.Read())
            {
                var user = new UserDto();
                user.Id = dataReader.GetInt32(0).ToString();
                user.Email = dataReader.GetString(1);
                user.FirstName = dataReader.GetString(2);
                user.LastName = dataReader.GetString(3);
                list.Add(user);
            }
            dbConnection.Close();
            return list;
        }

        public void Save(UserDto user)
        {
            IDbConnection dbConnection = _dataProvider.Db;
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            IDbCommand dbCommandInsertValue = dbConnection.CreateCommand(); // 9
            dbCommandInsertValue.CommandText = $"INSERT OR REPLACE INTO Users (id, email, firstName, lastName, photo) VALUES ({user.Id},'{user.Email}', '{user.FirstName}', '{user.LastName}', '{user.Photo}')"; // 10
            dbCommandInsertValue.ExecuteNonQuery();

            dbConnection.Close();
        }
    }
}
