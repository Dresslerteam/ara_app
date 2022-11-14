using Ara.Domain.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Repositories
{
    public class TaskPhotosRepository
    {
        public TaskPhotosRepository(IDataProvider dataProvider)
        {
            dataProvider = new SqlLiteDataProvider();
        }

        public List<TaskPhotoDAO> Get()
        {
            return new List<TaskPhotoDAO>();
        }

        public void Save(TaskPhotoDAO taskPhoto)
        {

        }

    }
}
