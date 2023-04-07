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
        public List<Job> _jobs;

        public MockJobsClient()
        {
            _jobs = this.GetJobs();
        }
        public async Task<Job> GetJobByIdAsync(string id)
        {
            var list = this._jobs;

            return await Task.FromResult(list.FirstOrDefault(l => l.Id == id));
        }

        public async Task<List<JobListItemDto>> GetJobsAsync()
        {

            var jobs = this._jobs;
            var jobsList = jobs.Select(j => new JobListItemDto(j)).ToList();

            return await Task.FromResult(jobsList);
        }

        public async Task<RepairManual> GetRepairManualByIdAsync(string id)
        {
            RepairManual Headliner_RepairManual = new RepairManual();

            return await Task.FromResult(Headliner_RepairManual);
        }

        private List<Job> GetJobs()
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
                    Year = 2022
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "Tomi",
                    LastName = "Martinez",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<TaskInfo>()
                {
                    RemovalTaskInfo,
                    InstallationTaskInfo
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
                    Year = 2018
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

            return list;
        }


        private static TaskInfo RemovalTaskInfo = new TaskInfo()
        {
            Id = 1,
            Title = "Front Bumper - Removal",
            Group = "Front Bumper",
            SequenceNumber = 1,
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
                                        ReferencedDocs = new List<(ManualStep.StepReferencedDocType Type, PdfDoc Doc)>()
                                        {
                                           (ManualStep.StepReferencedDocType.Procedure, new PdfDoc(){Url = "Front_Bumper_Impact_Bar_Removal_and_Installation.pdf", Title = "Front_Bumper_Impact_Bar_Removal_and_Installation.pdf"}),
                                           (ManualStep.StepReferencedDocType.Caution, new PdfDoc(){Url = "Fastener_Caution.pdf", Title = "Fastener_Caution.pdf"}),
                                        }

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
                                        Id = 25,
                                        Title = "Front Bumper Impact Bar Bracket (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "24.png",
                                            Url = "24.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 4,
                                Name = "Front Bumper Impact Bar Inner Bracket Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Impact_Bar_Inner_Bracket_Replacement",
                                    Url = "Front_Bumper_Impact_Bar_Inner_Bracket_Replacement_Document_ID_5630044.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 26,
                                        Title = "Front Bumper Impact Bar (1) » Remove — Front Bumper Impact Bar Removal and Installation",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "26.png",
                                            Url = "26.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 27,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "27.png",
                                            Url = "27.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 28,
                                        Title = "Front Fog Lamp Opening Cover (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "27.png",
                                            Url = "27.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 29,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "29.png",
                                            Url = "29.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 30,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "29.png",
                                            Url = "29.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 31,
                                        Title = "{ If equipped } Disconnect the electrical connector.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "29.png",
                                            Url = "29.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 32,
                                        Title = "Front Bumper Impact Bar Bolt - Outer (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "32.png",
                                            Url = "32.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 33,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "32.png",
                                            Url = "32.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 34,
                                        Title = "Front Bumper Impact Bar Bolt - Inner (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "34.png",
                                            Url = "34.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 35,
                                        Title = "Front Bumper Impact Bar Inner Bracket (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "34.png",
                                            Url = "34.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 5,
                                Name = "Front Bumper Impact Bar Outer Bracket Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Impact_Bar_Outer_Bracket_Replacement_Document",
                                    Url = "Front_Bumper_Impact_Bar_Outer_Bracket_Replacement_Document_ID_5629983.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 36,
                                        Title = "Front Bumper Impact Bar (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "26.png",
                                            Url = "26.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 37,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "27.png",
                                            Url = "27.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 38,
                                        Title = "Front Fog Lamp Opening Cover (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "27.png",
                                            Url = "27.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 39,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "29.png",
                                            Url = "29.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 40,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "29.png",
                                            Url = "29.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 41,
                                        Title = "{ If equipped } Disconnect the electrical connector.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "29.png",
                                            Url = "29.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 42,
                                        Title = "Front Bumper Impact Bar Bolt - Outer (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "34.png",
                                            Url = "34.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 43,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "34.png",
                                            Url = "34.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 6,
                                Name = "Front Bumper Outer Filler Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Outer_Filler_Replacement_Document",
                                    Url = "Front_Bumper_Outer_Filler_Replacement_Document_ID_5629448.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 44,
                                        Title = "Front Wheelhouse Liner Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "44.png",
                                            Url = "44.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 45,
                                        Title = "Front Wheelhouse Liner Retainer (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "44.png",
                                            Url = "44.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 46,
                                        Title = "Reposition the front wheelhouse liner (3) to gain access to the outer filler panel",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "44.png",
                                            Url = "44.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 47,
                                        Title = "Loosen the front bumper fascia guide bolt (1) from the engine compartment.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "47.png",
                                            Url = "47.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 48,
                                        Title = "Loosen the remaining 2 front bumper fascia guide bolts (1)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "48.png",
                                            Url = "48.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 49,
                                        Title = "Front Bumper Outer Filler Retainer (1) » Release [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "49.png",
                                            Url = "49.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 50,
                                        Title = "Front Bumper Outer Filler (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "49.png",
                                            Url = "49.png"
                                        },
                                        PhotoRequired = false,

                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 7,
                                Name = "Front Bumper Fascia Guide Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Guide_Replacement",
                                    Url = "Front_Bumper_Fascia_Guide_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 51,
                                        Title = "Front Wheelhouse Liner Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "44.png",
                                            Url = "44.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 52,
                                        Title = "Front Wheelhouse Liner Retainer (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "44.png",
                                            Url = "44.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 53,
                                        Title = "Reposition the front wheelhouse liner (3) to gain access to the outer filler panel",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "44.png",
                                            Url = "44.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 54,
                                        Title = "Remove the front bumper fascia guide bolt (1) from the engine compartment",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "47.png",
                                            Url = "47.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 55,
                                        Title = "Loosen the remaining 2 front bumper fascia guide bolts (1)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "48.png",
                                            Url = "48.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 56,
                                        Title = "Front Bumper Outer Filler Retainer (1) » Release [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "49.png",
                                            Url = "49.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 57,
                                        Title = "Front Bumper Outer Filler (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "49.png",
                                            Url = "49.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 58,
                                        Title = "Front Bumper Fascia Guide Bolt (1) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "58.png",
                                            Url = "58.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 59,
                                        Title = "Front Bumper Fascia Guide (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "58.png",
                                            Url = "58.png"
                                        },
                                        PhotoRequired = false,

                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 8,
                                Name = "Front Bumper Lower Shutter Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Lower_Shutter_Replacement_Document",
                                    Url = "Front_Bumper_Lower_Shutter_Replacement_Document_ID_5654340.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 60,
                                        Title = "With the aid of an assistant, install the impact bar (1). ",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "60.png",
                                            Url = "60.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 61,
                                        Title = "Radiator Air Lower Baffle Retainer (1) » Remove [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "61.png",
                                            Url = "61.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 62,
                                        Title = "Radiator Air Baffle Bracket Lower Bolt (1) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "62.png",
                                            Url = "62.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 63,
                                        Title = "Radiator Air Lower Baffle Bolt (1) » Remove [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "63.png",
                                            Url = "63.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 64,
                                        Title = "Remove the radiator air front lower baffle and radiator air lower baffle brackets as an assembly (1)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "64.png",
                                            Url = "64.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 65,
                                        Title = "Front Bumper Shutter Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "65.png",
                                            Url = "65.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 66,
                                        Title = "Remove the front bumper lower shutter (2)  from the radiator air front lower baffle",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "65.png",
                                            Url = "65.png"
                                        },
                                        PhotoRequired = false,

                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 9,
                                Name = "Front Bumper Lower Fascia Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Lower_Fascia_Replacement",
                                    Url = "Front_Bumper_Lower_Fascia_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 67,
                                        Title = "Raise and support the vehicle",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 68,
                                        Title = "Front Bumper Fascia Air Deflector Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 69,
                                        Title = "Pull the front bumper fascia air deflector (2) from the front impact bar.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 70,
                                        Title = "Front Bumper Impact Bar (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "70.png",
                                            Url = "70.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 71,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "71.png",
                                            Url = "71.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 72,
                                        Title = "Front Fog Lamp Opening Cover (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "71.png",
                                            Url = "71.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 73,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "73.png",
                                            Url = "73.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                    new ManualStep()
                                    {
                                        Id = 74,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "73.png",
                                            Url = "73.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 75,
                                        Title = "{ If equipped } Disconnect all electrical connectors.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "73.png",
                                            Url = "73.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 76,
                                        Title = "Front Bumper Impact Bar Outer Bracket Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "76.png",
                                            Url = "76.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 77,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "76.png",
                                            Url = "76.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 78,
                                        Title = "Front Bumper Impact Bar Bracket Inner Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "78.png",
                                            Url = "78.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 79,
                                        Title = "Front Bumper Impact Bar Bracket Inner (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "78.png",
                                            Url = "78.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 80,
                                        Title = "Front Bumper Lower Fascia Bolt (1) » Remove [20x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "80.png",
                                            Url = "80.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                    new ManualStep()
                                    {
                                        Id = 81,
                                        Title = "Front Bumper Lower Fascia (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "80.png",
                                            Url = "80.png"
                                        },
                                        PhotoRequired = false,

                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 10,
                                Name = "Front Bumper Shutter Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Lower_Shutter_Replacement_Document",
                                    Url = "Front_Bumper_Lower_Shutter_Replacement_Document_ID_5654340.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 82,
                                        Title = "Hood Latch Actuator Bolt (1) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "82.png",
                                            Url = "82.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 83,
                                        Title = "Hood Secondary Latch Release Handle (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "82.png",
                                            Url = "82.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 84,
                                        Title = "Intake Air Baffle Retainer (1) » Remove [10x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "84.png",
                                            Url = "84.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 85,
                                        Title = "Intake Air Splash Shield (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "84.png",
                                            Url = "84.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 86,
                                        Title = "Front Grille Bolt (1) » Remove [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "86.png",
                                            Url = "86.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 87,
                                        Title = "Front Grille Retainer (1) » Release [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "87.png",
                                            Url = "87.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 88,
                                        Title = "Front Grille Clip (2) » Release [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "87.png",
                                            Url = "87.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                    new ManualStep()
                                    {
                                        Id = 89,
                                        Title = "Front Grille (3) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "87.png",
                                            Url = "87.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 90,
                                        Title = "{ If equipped } Disconnect and reposition the lower shutter harness.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 91,
                                        Title = "Disconnect the electrical connector.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 92,
                                        Title = "Front Bumper Shutter Bolt (1) » Remove [9x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 93,
                                        Title = "Front Bumper Shutter (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 11,
                                Name = "Front Bumper Fascia Support Brace Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Support_Brace_Replacement",
                                    Url = "Front_Bumper_Fascia_Support_Brace_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 94,
                                        Title = "Raise the vehicle.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "95.png",
                                            Url = "95.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 95,
                                        Title = "Front Bumper Fascia Support Brace Bolt (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "95.png",
                                            Url = "95.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 96,
                                        Title = "Front Bumper Fascia Support Brace (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "95.png",
                                            Url = "95.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 12,
                                Name = "Front Bumper Fascia Air Deflector Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Support_Brace_Replacement",
                                    Url = "Front_Bumper_Fascia_Support_Brace_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 97,
                                        Title = "Raise the vehicle.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "97.png",
                                            Url = "97.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 98,
                                        Title = "Front Bumper Fascia Air Deflector Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "97.png",
                                            Url = "97.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 99,
                                        Title = "Pull the front bumper fascia air deflector (2) from the front impact bar",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "97.png",
                                            Url = "97.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 13,
                                Name = "Front Bumper Fascia Skid Plate Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Skid_Plate_Replacement",
                                    Url = "Front_Bumper_Fascia_Skid_Plate_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 100,
                                        Title = "Front Bumper Impact Bar (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "100.png",
                                            Url = "100.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 101,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "101.png",
                                            Url = "101.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 102,
                                        Title = "Front Fog Lamp Opening Cover (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "101.png",
                                            Url = "101.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 103,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "103.png",
                                            Url = "103.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 104,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "103.png",
                                            Url = "103.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 105,
                                        Title = "{ If equipped } Disconnect the electrical connector. [2x",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "103.png",
                                            Url = "103.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 106,
                                        Title = "Front Bumper Impact Bar Outer Bracket Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "106.png",
                                            Url = "106.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 107,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "106.png",
                                            Url = "106.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 108,
                                        Title = "Front Bumper Impact Bar Bracket Inner Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "108.png",
                                            Url = "108.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 109,
                                        Title = "Front Bumper Impact Bar Bracket Inner (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "108.png",
                                            Url = "108.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 110,
                                        Title = "Using a trim type tool, release the front bumper fascia skid plate retainers.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "110.png",
                                            Url = "110.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 111,
                                        Title = "Front Bumper Fascia Skid Plate (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "110.png",
                                            Url = "110.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            }
                        }
        };

        private static TaskInfo InstallationTaskInfo = new TaskInfo()
        {
            Id = 2,
            Title = "Front Bumper - Installation",
            Group = "Front Bumper",
            SequenceNumber = 2,
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
                                        Title = "Connect electrical connections, if applicable.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i1.png",
                                            Url = "i1.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 2,
                                        Title = "With the aid of an assistant, install the impact bar (1).",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i1.png",
                                            Url = "i1.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 3,
                                        Title = "Front Bumper Fascia Support Brace Bolt (1) install and tighten",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i2.png",
                                            Url = "i2.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 4,
                                        Title = "Front Bumper Fascia Center Support Bolt (2) Install and tighten [4x] ",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i2.png",
                                            Url = "i2.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 5,
                                        Title = "Front Bumper Impact Bar Bolt (1) Install and tighten [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i5.png",
                                            Url = "i5.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 6,
                                        Title = "Front Bumper Shutter (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i6.png",
                                            Url = "i6.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 7,
                                        Title = "Front Bumper Shutter Bolt (1) Install and tighten [9x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i6.png",
                                            Url = "i6.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 8,
                                        Title = "Connect the electrical connector.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i6.png",
                                            Url = "i6.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 9,
                                        Title = "{ If equipped } Connect and install the lower shutter harness",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i6.png",
                                            Url = "i6.png"
                                        },
                                        PhotoRequired = true
                                    },
                                    new ManualStep()
                                    {
                                        Id = 10,
                                        Title = "Front Grille  (3) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i10.png",
                                            Url = "i10.png"
                                        },
                                        PhotoRequired = true
                                    },
                                     new ManualStep()
                                    {
                                        Id = 11,
                                        Title = "Front Grille Clip  (2) » Install [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i10.png",
                                            Url = "i10.png"
                                        },
                                        PhotoRequired = true
                                    },
                                      new ManualStep()
                                    {
                                        Id = 12,
                                        Title = "Front Grille Retainer (1) » Install [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i10.png",
                                            Url = "i10.png"
                                        },
                                        PhotoRequired = true
                                    },
                                       new ManualStep()
                                    {
                                        Id = 13,
                                        Title = "Front Grille Bolt  (1)  » Install and tighten [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i13.png",
                                            Url = "i13.png"
                                        },
                                        PhotoRequired = true
                                    },
                                        new ManualStep()
                                    {
                                        Id = 14,
                                        Title = "Intake Air Splash Shield (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i14.png",
                                            Url = "i14.png"
                                        },
                                        PhotoRequired = true
                                    },
                                        new ManualStep()
                                    {
                                        Id = 15,
                                        Title = "Intake Air Baffle Retainer (1) » Install [10x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i14.png",
                                            Url = "i14.png"
                                        },
                                        PhotoRequired = true
                                    },
                                        new ManualStep()
                                    {
                                        Id = 16,
                                        Title = "Hood Secondary Latch Release Handle (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i16.png",
                                            Url = "i16.png"
                                        },
                                        PhotoRequired = false
                                    },
                                        new ManualStep()
                                    {
                                        Id = 17,
                                        Title = "Hood Latch Actuator Bolt (1) » Install and tighten [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i16.png",
                                            Url = "i16.png"
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
                                        Title = "Tighten the 6 impact bar adjustment bracket bolts (1)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i18.png",
                                            Url = "i18.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 19,
                                        Title = "Front Bumper Impact Bar (1) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i19.png",
                                            Url = "i19.png"
                                        },
                                        PhotoRequired = false,
                                        ReferencedDocs = new List<(ManualStep.StepReferencedDocType Type, PdfDoc Doc)>()
                                        {
                                           (ManualStep.StepReferencedDocType.Procedure, new PdfDoc(){Url = "Front_Bumper_Impact_Bar_Removal_and_Installation.pdf", Title = "Front_Bumper_Impact_Bar_Removal_and_Installation.pdf"}),
                                           (ManualStep.StepReferencedDocType.Caution, new PdfDoc(){Url = "Fastener_Caution.pdf", Title = "Fastener_Caution.pdf"}),
                                        }

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
                                        Id = 20,
                                        Title = "Front Bumper Impact Bar Bracket (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i20.png",
                                            Url = "i20.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 21,
                                        Title = "Front Bumper Impact Bar Bracket Bolt (1) » Install and tighten [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i20.png",
                                            Url = "i20.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 22,
                                        Title = "{ If equipped } Radiator Air Lower Baffle Bolt (1) » Install and tighten [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i22.png",
                                            Url = "i22.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 23,
                                        Title = "Front Bumper Impact Bar (1) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i23.png",
                                            Url = "i23.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 4,
                                Name = "Front Bumper Impact Bar Inner Bracket Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Impact_Bar_Inner_Bracket_Replacement",
                                    Url = "Front_Bumper_Impact_Bar_Inner_Bracket_Replacement_Document_ID_5630044.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 24,
                                        Title = "Front Bumper Impact Bar Inner Bracket (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i24.png",
                                            Url = "i24.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 25,
                                        Title = "Front Bumper Impact Bar Bolt - Inner (1) » Install and tighten [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i24.png",
                                            Url = "i24.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 26,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i26.png",
                                            Url = "i26.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 27,
                                        Title = "Front Bumper Impact Bar Bolt - Outer (1) » Install and tighten [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i26.png",
                                            Url = "i26.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 28,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i28.png",
                                            Url = "i28.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 29,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Install and tighten [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i28.png",
                                            Url = "i28.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 30,
                                        Title = "{ If equipped } Connect the electrical connector",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i28.png",
                                            Url = "i28.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 31,
                                        Title = "Front Fog Lamp Opening Cover (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i31.png",
                                            Url = "i31.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 32,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Install and tighten [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i31.png",
                                            Url = "i31.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 33,
                                        Title = "Front Bumper Impact Bar (1) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i33.png",
                                            Url = "i33.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 5,
                                Name = "Front Bumper Impact Bar Outer Bracket Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Impact_Bar_Outer_Bracket_Replacement_Document",
                                    Url = "Front_Bumper_Impact_Bar_Outer_Bracket_Replacement_Document_ID_5629983.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 34,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i34.png",
                                            Url = "i34.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 35,
                                        Title = "Front Bumper Impact Bar Outer Bracket Bolt (1) » Install and tighten [6x] ",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i34.png",
                                            Url = "i34.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 36,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i36.png",
                                            Url = "i36.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 37,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Install and tighten [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i36.png",
                                            Url = "i36.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 38,
                                        Title = "{ If equipped } Connect the electrical connector.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i36.png",
                                            Url = "i36.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 39,
                                        Title = "Front Fog Lamp Opening Cover (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i39.png",
                                            Url = "i39.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 40,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Install and tighten [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i39.png",
                                            Url = "i39.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 41,
                                        Title = "Front Bumper Impact Bar (1) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i41.png",
                                            Url = "i41.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 6, 
                                Name = "Front Bumper Outer Filler Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Outer_Filler_Replacement_Document",
                                    Url = "Front_Bumper_Outer_Filler_Replacement_Document_ID_5629448.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 42,
                                        Title = "Front Bumper Outer Filler (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i42.png",
                                            Url = "i42.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 43,
                                        Title = "Front Bumper Outer Filler Retainer (1) » Install [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i42.png",
                                            Url = "i42.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 44,
                                        Title = "Front Bumper Fascia Guide Bolt (1) » Tighten [2x] ",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i44.png",
                                            Url = "i44.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 45,
                                        Title = "Front Bumper Fascia Guide Bolt (1) » Tighten",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i45.png",
                                            Url = "i45.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 46,
                                        Title = "Front Wheelhouse Liner (3) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i46.png",
                                            Url = "i46.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 47,
                                        Title = "Front Wheelhouse Liner Retainer (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i46.png",
                                            Url = "i46.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 48,
                                        Title = "Front Wheelhouse Liner Bolt (1) » Install and tighten [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i46.png",
                                            Url = "i46.png"
                                        },
                                        PhotoRequired = false,

                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 7,
                                Name = "Front Bumper Fascia Guide Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Guide_Replacement",
                                    Url = "Front_Bumper_Fascia_Guide_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 49,
                                        Title = "Front Bumper Fascia Guide (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i49.png",
                                            Url = "i49.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 50,
                                        Title = "Front Bumper Fascia Guide Bolt (1) » Loosely install [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i49.png",
                                            Url = "i49.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 51,
                                        Title = "Front Bumper Outer Filler (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i51.png",
                                            Url = "i51.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 52,
                                        Title = "Front Bumper Outer Filler Retainer (1) » Install [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i51.png",
                                            Url = "i51.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 53,
                                        Title = "Front Bumper Fascia Guide Bolt (1) » Tighten [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i53.png",
                                            Url = "i53.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 54,
                                        Title = "Front Bumper Fascia Guide Bolt (1) » Install and tighten",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i54.png",
                                            Url = "i54.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 55,
                                        Title = "Front Wheelhouse Liner (3) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i55.png",
                                            Url = "i55.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 56,
                                        Title = "Front Wheelhouse Liner Retainer (2) » Install",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i55.png",
                                            Url = "i55.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 57,
                                        Title = "Front Wheelhouse Liner Bolt (1) » Install and tighten [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i55.png",
                                            Url = "i55.png"
                                        },
                                        PhotoRequired = false,

                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 8, 
                                Name = "Front Bumper Lower Shutter Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Lower_Shutter_Replacement_Document",
                                    Url = "Front_Bumper_Lower_Shutter_Replacement_Document_ID_5654340.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 58,
                                        Title = "Install the front bumper lower shutter (2)  to the radiator air front lower baffle.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i58.png",
                                            Url = "i58.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 59,
                                        Title = "Front Bumper Shutter Bolt (1) » Install and tighten [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i58.png",
                                            Url = "i58.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 60,
                                        Title = "Install the radiator air front lower baffle and radiator air lower baffle brackets as an assembly (1).",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i60.png",
                                            Url = "i60.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 61,
                                        Title = "Radiator Air Lower Baffle Bolt (1) » Install and tighten [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i61.png",
                                            Url = "i61.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 62,
                                        Title = "Radiator Air Baffle Bracket Lower Bolt (1) » Install and tighten [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i62.png",
                                            Url = "i62.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 63,
                                        Title = "Radiator Air Lower Baffle Retainer (1) » Install [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i63.png",
                                            Url = "i63.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 64,
                                        Title = "With the aid of an assistant, install the impact bar (1)",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "i64.png",
                                            Url = "i64.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 9,//todo:tat
                                Name = "Front Bumper Lower Fascia Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Lower_Fascia_Replacement",
                                    Url = "Front_Bumper_Lower_Fascia_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 65,
                                        Title = "Raise and support the vehicle",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 66,
                                        Title = "Front Bumper Fascia Air Deflector Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 67,
                                        Title = "Raise and support the vehicle",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 68,
                                        Title = "Front Bumper Fascia Air Deflector Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 69,
                                        Title = "Pull the front bumper fascia air deflector (2) from the front impact bar.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "68.png",
                                            Url = "68.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 70,
                                        Title = "Front Bumper Impact Bar (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "70.png",
                                            Url = "70.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 71,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "71.png",
                                            Url = "71.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 72,
                                        Title = "Front Fog Lamp Opening Cover (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "71.png",
                                            Url = "71.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 73,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "73.png",
                                            Url = "73.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                    new ManualStep()
                                    {
                                        Id = 74,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "73.png",
                                            Url = "73.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 75,
                                        Title = "{ If equipped } Disconnect all electrical connectors.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "73.png",
                                            Url = "73.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 76,
                                        Title = "Front Bumper Impact Bar Outer Bracket Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "76.png",
                                            Url = "76.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 77,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "76.png",
                                            Url = "76.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 78,
                                        Title = "Front Bumper Impact Bar Bracket Inner Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "78.png",
                                            Url = "78.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 79,
                                        Title = "Front Bumper Impact Bar Bracket Inner (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "78.png",
                                            Url = "78.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 80,
                                        Title = "Front Bumper Lower Fascia Bolt (1) » Remove [20x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "80.png",
                                            Url = "80.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                    new ManualStep()
                                    {
                                        Id = 81,
                                        Title = "Front Bumper Lower Fascia (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "80.png",
                                            Url = "80.png"
                                        },
                                        PhotoRequired = false,

                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 10,
                                Name = "Front Bumper Shutter Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Lower_Shutter_Replacement_Document",
                                    Url = "Front_Bumper_Lower_Shutter_Replacement_Document_ID_5654340.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 82,
                                        Title = "Hood Latch Actuator Bolt (1) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "82.png",
                                            Url = "82.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 83,
                                        Title = "Hood Secondary Latch Release Handle (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "82.png",
                                            Url = "82.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 84,
                                        Title = "Intake Air Baffle Retainer (1) » Remove [10x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "84.png",
                                            Url = "84.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 85,
                                        Title = "Intake Air Splash Shield (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "84.png",
                                            Url = "84.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 86,
                                        Title = "Front Grille Bolt (1) » Remove [4x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "86.png",
                                            Url = "86.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 87,
                                        Title = "Front Grille Retainer (1) » Release [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "87.png",
                                            Url = "87.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 88,
                                        Title = "Front Grille Clip (2) » Release [8x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "87.png",
                                            Url = "87.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                    new ManualStep()
                                    {
                                        Id = 89,
                                        Title = "Front Grille (3) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "87.png",
                                            Url = "87.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 90,
                                        Title = "{ If equipped } Disconnect and reposition the lower shutter harness.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 91,
                                        Title = "Disconnect the electrical connector.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 92,
                                        Title = "Front Bumper Shutter Bolt (1) » Remove [9x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 93,
                                        Title = "Front Bumper Shutter (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "90.png",
                                            Url = "90.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 11,
                                Name = "Front Bumper Fascia Support Brace Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Support_Brace_Replacement",
                                    Url = "Front_Bumper_Fascia_Support_Brace_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 94,
                                        Title = "Raise the vehicle.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "95.png",
                                            Url = "95.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 95,
                                        Title = "Front Bumper Fascia Support Brace Bolt (1) » Remove [3x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "95.png",
                                            Url = "95.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 96,
                                        Title = "Front Bumper Fascia Support Brace (2) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "95.png",
                                            Url = "95.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 12,
                                Name = "Front Bumper Fascia Air Deflector Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Support_Brace_Replacement",
                                    Url = "Front_Bumper_Fascia_Support_Brace_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 97,
                                        Title = "Raise the vehicle.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "97.png",
                                            Url = "97.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 98,
                                        Title = "Front Bumper Fascia Air Deflector Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "97.png",
                                            Url = "97.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 99,
                                        Title = "Pull the front bumper fascia air deflector (2) from the front impact bar",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "97.png",
                                            Url = "97.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            },
                            new RepairManual()
                            {
                                Id = 13,
                                Name = "Front Bumper Fascia Skid Plate Replacement",
                                Document = new PdfDoc()
                                {
                                    Title= "Front_Bumper_Fascia_Skid_Plate_Replacement",
                                    Url = "Front_Bumper_Fascia_Skid_Plate_Replacement.pdf"
                                },
                                Steps = new List<ManualStep>()
                                {
                                    new ManualStep()
                                    {
                                        Id = 100,
                                        Title = "Front Bumper Impact Bar (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "100.png",
                                            Url = "100.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 101,
                                        Title = "Front Fog Lamp Opening Cover Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "101.png",
                                            Url = "101.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 102,
                                        Title = "Front Fog Lamp Opening Cover (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "101.png",
                                            Url = "101.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 103,
                                        Title = "{ If equipped } Front Fog Lamp Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "103.png",
                                            Url = "103.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 104,
                                        Title = "{ If equipped } Front Fog Lamp (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "103.png",
                                            Url = "103.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 105,
                                        Title = "{ If equipped } Disconnect the electrical connector. [2x",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "103.png",
                                            Url = "103.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 106,
                                        Title = "Front Bumper Impact Bar Outer Bracket Bolt (1) » Remove [12x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "106.png",
                                            Url = "106.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 107,
                                        Title = "Front Bumper Impact Bar Outer Bracket (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "106.png",
                                            Url = "106.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 108,
                                        Title = "Front Bumper Impact Bar Bracket Inner Bolt (1) » Remove [6x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "108.png",
                                            Url = "108.png"
                                        },
                                        PhotoRequired = false
                                    },
                                     new ManualStep()
                                    {
                                        Id = 109,
                                        Title = "Front Bumper Impact Bar Bracket Inner (2) » Remove [2x]",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "108.png",
                                            Url = "108.png"
                                        },
                                        PhotoRequired = false
                                    },
                                    new ManualStep()
                                    {
                                        Id = 110,
                                        Title = "Using a trim type tool, release the front bumper fascia skid plate retainers.",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "110.png",
                                            Url = "110.png"
                                        },
                                        PhotoRequired = false,

                                    },
                                     new ManualStep()
                                    {
                                        Id = 111,
                                        Title = "Front Bumper Fascia Skid Plate (1) » Remove",
                                        Image = new ManualStep.StepImage
                                        {
                                            Title = "110.png",
                                            Url = "110.png"
                                        },
                                        PhotoRequired = false
                                    }
                                }
                            }
                        }
        };
    }
}
