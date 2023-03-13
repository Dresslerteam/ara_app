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
                        Title = "Front Bumper - Removal",
                        Group = "Front Bumper",
                        SequenceNumber =1,
                        Status = TaskInfo.TaskStatus.ToDo,
                         RepairManuals = new List<RepairManual>()
                        {
                            new RepairManual()
                            {
                                Id = 1,
                                Name = "Front Bumper Impact Bar Removal and Installation",
                                Document = new PdfDoc()
                                {
                                    Title= "Front Bumper Impact Bar Removal and Installation",
                                    Url = "Front_Bumper_Impact_Bar_Removal_and_Installation.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 1,
                                        Title = "Hood Latch Actuator Bolt (1) >> Remove [2x] (1)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "1.png",
                                            Url = "1.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 2,
                                        Title = "Hood Secondary Latch Release Handle (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "1.png",
                                            Url = "1.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 3,
                                        Title = "Intake Air Baffle Retainer (1) » Remove [10x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "2.png",
                                            Url = "2.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 4,
                                        Title = "Intake Air Splash Shield (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "2.png",
                                            Url = "2.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 5,
                                        Title = "Front Grille Bolt (1) » Remove [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "3.png",
                                            Url = "3.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 6,
                                        Title = "Front Grille Retainer (1) » Release [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "4.png",
                                            Url = "4.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 7,
                                        Title = "Front Grille Clip (2) » Release [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "4.png",
                                            Url = "4.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 8,
                                        Title = "Front Grille (3) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "4.png",
                                            Url = "4.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 9,
                                        Title = "{ If equipped } Disconnect and reposition the lower shutter harness.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "5.png",
                                            Url = "5.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 10,
                                        Title = "Disconnect the electrical connector.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "5.png",
                                            Url = "5.png"
                                        },
                                        PhotoRequired = true
                                    },
                                     new ManualStep()
                                    {
                                        Id = 11,
                                        Title = "Front Bumper Shutter Bolt (1) » Remove [9x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "5.png",
                                            Url = "5.png"
                                        },
                                        PhotoRequired = true
                                    },
                                      new ManualStep()
                                    {
                                        Id = 12,
                                        Title = "Front Bumper Shutter (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "5.png",
                                            Url = "5.png"
                                        },
                                        PhotoRequired = true
                                    },
                                       new ManualStep()
                                    {
                                        Id = 13,
                                        Title = "Front Bumper Impact Bar Bolt (1) » Remove [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "6.png",
                                            Url = "6.png"
                                        },
                                        PhotoRequired = true
                                    },
                                        new ManualStep()
                                    {
                                        Id = 14,
                                        Title = "Remove the left and right front bumper fascia support brace bolts (1)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "14.png",
                                            Url = "14.png"
                                        },
                                        PhotoRequired = true
                                    },
                                        new ManualStep()
                                    {
                                        Id = 15,
                                        Title = "Remove the 2 left and right front bumper fascia center support bolts (2)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "14.png",
                                            Url = "14.png"
                                        },
                                        PhotoRequired = true
                                    },
                                        new ManualStep()
                                    {
                                        Id = 16,
                                        Title = "With the aid of an assistant remove the fascia (1).",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "16.png",
                                            Url = "16.png"
                                        },
                                        PhotoRequired = false
                                    },
                                        new ManualStep()
                                    {
                                        Id = 17,
                                        Title = "Disconnect electrical connections, if applicable.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "16.png",
                                            Url = "16.png"
                                        },
                                        PhotoRequired = true
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 2,
                                Name = "Front Bumper Impact Bar Adjustment",
                                Document = new PdfDoc()
                                {
                                    Title= "Front Bumper Impact Bar Adjustment",
                                    Url = "Front_Bumper_Impact_Bar_Adjustment_Document_ID_5633756.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 18,
                                        Title = "Measure and document the impact bar to headlamps dimensions and the impact bar to fenders dimensions and note how far to move the impact bar right, left, up, and down to achieve an equal gap to the fenders and headlamps.",
                                        Image = null, //todo:what if there is not step photo ? 
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 19,
                                        Title = "Front Bumper Impact Bar (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "19.png",
                                            Url = "19.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 20,
                                        Title = "With a grease pencil, mark the top and side bumper adjustment bracket (2).",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "20.png",
                                            Url = "20.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 21,
                                        Title = "Loosen the 6 impact bar adjustment bracket bolts (1) and move the 2 adjustment brackets (2) to the pre determined measurements taken",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "20.png",
                                            Url = "20.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 3,
                                Name = "Front Bumper Impact Bar Bracket Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front Bumper Impact Bar Bracket Replacement",
                                    Url = "Front_Bumper_Impact_Bar_Bracket_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 22,
                                        Title = "Front Bumper Impact Bar (1) » Remove ",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "22.png",
                                            Url = "22.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 23,
                                        Title = "{ If equipped } Radiator Air Lower Baffle Bolt (1) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "23.png",
                                            Url = "23.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 24,
                                        Title = "Front Bumper Impact Bar Bracket Bolt (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "24.png",
                                            Url = "24.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 24,
                                        Title = "Front Bumper Impact Bar Bracket (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "24.png",
                                            Url = "24.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            }
                        }
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "Front Bumper - Installation",
                        Group = "Front Bumper",
                        SequenceNumber = 1,
                        Status = TaskInfo.TaskStatus.ToDo,
                       
                    }
                },
                PreliminaryEstimation = new PdfDoc()
                {
                    Title = "Estimation",
                    Url = "estimation_1.pdf"
                },
                PreliminaryScan = new PdfDoc()
                {
                    Title = "PreliminaryScan",
                    Url = "preliminary_scan_1.pdf"
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
                },
                PreliminaryEstimation = new PdfDoc()
                {
                    Title = "Estimation",
                    Url = "estimation_1.pdf"
                },
                PreliminaryScan = new PdfDoc()
                {
                    Title = "PreliminaryScan",
                    Url = "preliminary_scan_1.pdf"
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
                CarOwner = new CarOwner()
                {
                    FirstName = "Tomi",
                    LastName = "Martinez",
                    Mobile = "(406) 555-5555"
                },
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "CHEVROLET",
                    Model = "SILVERADO ",
                    Vin = "3GCUYGEDXNG211028",
                    Year = 2022
                },
                PreliminaryEstimation = new PdfDoc()
                {
                    Title = "Estimation",
                    Url = "estimation_1.pdf"
                },
                PreliminaryScan = new PdfDoc()
                {
                    Title = "PreliminaryScan",
                    Url = "preliminary_scan_1.pdf"
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
                CarOwner = new CarOwner()
                {
                    FirstName = "Patricio",
                    LastName = "Delgado",
                    Mobile = "(406) 555-5555"
                },
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "CHEVROLET",
                    Model = "TRAVERSE ",
                    Vin = "1GNERJKX0KJ221758",
                    Year = 2019
                },
                PreliminaryEstimation = new PdfDoc()
                {
                    Title = "Estimation",
                    Url = "estimation_1.pdf"
                },
                PreliminaryScan = new PdfDoc()
                {
                    Title = "PreliminaryScan",
                    Url = "preliminary_scan_1.pdf"
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
