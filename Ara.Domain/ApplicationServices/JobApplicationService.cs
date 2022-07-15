using Ara.Domain.Dtos;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
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

        public static RepairManual Headliner_RepairManual = new RepairManual()
        {
            Id = 1,
            Name = "Headliner",
            Category = RepairManual.RepairManualCategory.RemovalAndInstallation,
            Description = "Base Part Number: 5451916",
            Materials = new List<Material>() { new Material() { Name = "3M™ Super-Fast Repair Adhesive 04747" } },
            DocumentUrl = "https://drive.google.com/file/d/1VXKL1C5P53o7rl9TdlWup4pNPkKVZjtn/view",
            StepsGroups = new List<StepsGroup>()
            {
                new StepsGroup()
                {
                    Name = "Removal",
                    Messages = new List<Common.Message>()
                    {
                        new Common.Message(){Type = Common.Message.MessageType.Note, Text = "Removal steps in this procedure may contain installation details."}
                    },
                    Steps = new List<ManualStep>()
                    {
                        new ManualStep()
                        {
                            Id = 1,
                            Title = "Remove the following items",
                            Labels = new List<string> { "All vehicles" },
                            SubSteps = new List<ManualStep>()
                            {
                                 new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "On both sides, remove the A-pillar trim panels",
                                    ReferencedManualId = 2
                                 }
                            }
                        }
                    }
                }
            }
        };
    }
}
