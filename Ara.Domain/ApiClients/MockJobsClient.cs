using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Interfaces;
using Ara.Domain.Common;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ara.Domain.ApiClients
{
    public class MockJobsClient : IJobsClient
    {
        public async Task<Job> GetJobByIdAsync(string id)
        {
            var job1 = new Job()
            {
                Id = "1",
                RepairOrderNo = "50518",
                ClaimNo = "282584692314B16",
                EstimatorFullName = "Chad Streck",
                CreatedOn = DateTime.Now,
                Status = Job.JobStatus.ToDo,
                Car = new Car()
                {
                    Manufacturer = "CHEVROLET",
                    Model = "SILVERADO",
                    Vin = "3GCUYGEDXNG211028",
                    Year = "2022"
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "Tomi",
                    LastName = "Martinez",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<TaskInfo>()
                {
                    new TaskInfo()
                    {
                        Id = 1,
                        Title = "Front Bumper - Removal"
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "Front Bumper - Installation"
                    }
                }
            };


            var job2 = new Job()
            {
                Id = "2",
                RepairOrderNo = "50519",
                ClaimNo = "282584692314B16",
                EstimatorFullName = "Chad Streck",
                CreatedOn = DateTime.Now,
                Status = Job.JobStatus.InProgress,
                Car = new Car()
                {
                    Manufacturer = "Nissan",
                    Model = "Skyline",
                    Vin = "1FM5K8D8XJGA43957",
                    Year = "2018"
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "Patricio",
                    LastName = "Delgado",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<TaskInfo>()
                {
                    new TaskInfo()
                    {
                        Id = 1,
                        Title = "Front Bumper - Removal"
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "Front Bumper - Installation"
                    }
                }
            };



            var list = new List<Job>() { job1, job2 };

            return await Task.FromResult(list.FirstOrDefault(l => l.Id == id));
        }

        public async Task<List<JobListItemDto>> GetJobsAsync()
        {
            var job1 = new JobListItemDto()
            {
                Id = "1",
                RepairOrderNo = "50518",
                ClaimNo = "282584692314B16",
                EstimatorFullName = "Chad Streck",
                Status = Job.JobStatus.ToDo,
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "CHEVROLET",
                    Model = "SILVERADO ",
                    Vin = "3GCUYGEDXNG211028",
                    Year = 2022
                }
            };


            var job2 = new JobListItemDto()
            {
                Id = "2",
                RepairOrderNo = "50519",
                ClaimNo = "282584692314B16",
                EstimatorFullName = "Chad Streck",
                Status = Job.JobStatus.InProgress,
                NumberOfDoneTasks = 1,
                NumberOfTasks = 10,
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "CHEVROLET",
                    Model = "TRAVERSE ",
                    Vin = "1GNERJKX0KJ221758",
                    Year = 2019
                }
            };



            return await Task.FromResult(new List<JobListItemDto>() { job1, job2 });
        }

        public async Task<RepairManual> GetRepairManualByIdAsync(string id)
        {
            RepairManual Headliner_RepairManual = new RepairManual();

            return await Task.FromResult(Headliner_RepairManual);
        }
    }
}
