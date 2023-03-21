using Ara.Domain.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ara.Domain.Repositories
{


    public class TaskPhotosRepository
    {
        private readonly IDataProvider _dataProvider;
        public TaskPhotosRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<TaskPhotoDAO> Get()
        {
            return new List<TaskPhotoDAO>();
        }

        public void Save(TaskPhotoDAO taskPhoto)
        {
            IDbConnection dbConnection = _dataProvider.Db; 
            dbConnection.Open();
            IDbCommand dbCommandInsertValue = dbConnection.CreateCommand(); // 9
            //dbCommandInsertValue.CommandText = "INSERT OR REPLACE INTO HitCountTableSimple (id, hits) VALUES (0, " + hitCount + ")"; // 10
            dbCommandInsertValue.ExecuteNonQuery(); 

            dbConnection.Close(); 
        }

    }
}
