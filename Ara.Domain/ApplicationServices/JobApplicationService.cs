using Ara.Domain.Dtos;
using Ara.Domain.JobManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApplicationServices
{
    public class JobApplicationService
    {
        public List<JobListItemDto> GetJobs()
        {
            var list = new List<JobListItemDto>(){
                new JobListItemDto()
            {
                    Id = 1,
                    Number = "AB1230",
                    Progress = 0,
                    Status = Job.JobStatus.ToDo,
                    CarInfo = new JobListItemDto.CarDto()
                    {
                        Manufacturer = "Ford",
                        Model = "Explorer",
                        Vin = "1FM5K7D81JGA97674",
                        Year = 2018
                    }

            },
                new JobListItemDto()
            {
                    Id = 2,
                    Number = "AB1231",
                    Progress = 0,
                    Status = Job.JobStatus.ToDo,
                    CarInfo = new JobListItemDto.CarDto()
                    {
                        Manufacturer = "Ford",
                        Model = "Explorer",
                        Vin = "1FM5K7D81JGA97674",
                        Year = 2018
                    }
            },
                 new JobListItemDto()
            {
                    Id = 3,
                    Number = "AB13234",
                    Progress = 0,
                    Status = Job.JobStatus.ToDo,
                    CarInfo = new JobListItemDto.CarDto()
                    {
                        Manufacturer = "Ford",
                        Model = "Explorer",
                        Vin = "1FM5K7D81JGA97674",
                        Year = 2018
                    }
            },
                  new JobListItemDto()
            {
                    Id = 4,
                    Number = "AB1236",
                    Progress = 0,
                    Status = Job.JobStatus.ToDo,
                    CarInfo = new JobListItemDto.CarDto()
                    {
                        Manufacturer = "Ford",
                        Model = "Explorer",
                        Vin = "1FM5K7D81JGA97674",
                        Year = 2018
                    }
            }
            };
            return new List<JobListItemDto>();
        }


        public Job GetJob(int id)
        {
            return new Job();
        }
    }
}
