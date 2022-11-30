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
                Code = "123",
                CreatedOn = DateTime.Now,
                Status = Job.JobStatus.InProgress,
                Title = "Collision 1",
                Car = new Car()
                {
                    Manufacturer = "Ford",
                    Model = "Explorer",
                    Vin = "1FM5K7D81JGA97674",
                    Year = "2019"
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "Tato Khorava",
                    LastName = "Moss",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<TaskInfo>()
                {
                    new TaskInfo()
                    {
                        Id = 1,
                        Title = "Inspections after collision"
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "PrepVehicle for repairs"
                    },
                      new TaskInfo()
                    {
                        Id = 3,
                        Title = "R&I Quarter Panel",
                        RepairManualId = "6307dea4e92fe007aee9c57c"
                    }
                }
            };


            var job2 = new Job()
            {
                Id = "2",
                Code = "",
                CreatedOn = DateTime.Now,
                Status = Job.JobStatus.InProgress,
                Title = "Collision 2",
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
                        Title = "Inspections after collision"
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "PrepVehicle for repairs"
                    },
                      new TaskInfo()
                    {
                        Id = 3,
                        Title = "R&I Quarter Panel"
                    }
                }
            };

            var job3 = new Job()
            {
                Id = "3",
                Code = "",
                CreatedOn = DateTime.Now,
                Status = Job.JobStatus.InProgress,
                Title = "Collision 1",
                Car = new Car()
                {
                    Manufacturer = "Ford",
                    Model = "Mustang",
                    Vin = "1FM5K8D8XJGA43957",
                    Year = "2018"
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "T",
                    LastName = "Moss",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<TaskInfo>()
                {
                    new TaskInfo()
                    {
                        Id = 1,
                        Title = "Inspections after collision"
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "PrepVehicle for repairs"
                    },
                      new TaskInfo()
                    {
                        Id = 3,
                        Title = "R&I Quarter Panel"
                    }
                }
            };

            var job4 = new Job()
            {
                Id = "4",
                Code = "",
                CreatedOn = DateTime.Now,
                Status = Job.JobStatus.InProgress,
                Title = "Collision 4",
                Car = new Car()
                {
                    Manufacturer = "BMW",
                    Model = "X6",
                    Vin = "1FM5K8D8XJGA43957",
                    Year = "2018"
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "T",
                    LastName = "Moss",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<TaskInfo>()
                {
                    new TaskInfo()
                    {
                        Id = 1,
                        Title = "Inspections after collision"
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "PrepVehicle for repairs"
                    },
                      new TaskInfo()
                    {
                        Id = 3,
                        Title = "R&I Quarter Panel"
                    }
                }
            };

            var job5 = new Job()
            {
                Id = "5",
                Code = "",
                CreatedOn = DateTime.Now,
                Status = Job.JobStatus.Completed,
                Title = "Collision 5",
                Car = new Car()
                {
                    Manufacturer = "Lexus",
                    Model = "RX450",
                    Vin = "1FM5K7D81JGA97674",
                    Year = "2019"
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "T",
                    LastName = "Moss",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<TaskInfo>()
                {
                    new TaskInfo()
                    {
                        Id = 1,
                        Title = "Inspections after collision"
                    },
                     new TaskInfo()
                    {
                        Id = 2,
                        Title = "PrepVehicle for repairs"
                    }
                }
            };

            var list = new List<Job>() { job1, job2, job3, job4, job5 };

            return await Task.FromResult(list.FirstOrDefault(l => l.Id == id));
        }

        public async Task<List<JobListItemDto>> GetJobsAsync()
        {
            var job1 = new JobListItemDto()
            {
                Id = "1",
                Code = "123",
                Status = Job.JobStatus.InProgress,
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "Ford",
                    Model = "Explorer",
                    Vin = "1FM5K7D81JGA97674",
                    Year = 2019
                }
            };


            var job2 = new JobListItemDto()
            {
                Id = "2",
                Code = "",
                Status = Job.JobStatus.InProgress,
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "Nissan",
                    Model = "Skyline",
                    Vin = "1FM5K8D8XJGA43957",
                    Year = 2018
                }
            };

            var job3 = new JobListItemDto()
            {
                Id = "3",
                Code = "",
                Status = Job.JobStatus.InProgress,
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "Ford",
                    Model = "Mustang",
                    Vin = "1FM5K8D8XJGA43957",
                    Year = 2018
                }
            };

            var job4 = new JobListItemDto()
            {
                Id = "4",
                Code = "",
                Status = Job.JobStatus.InProgress,
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "BMW",
                    Model = "X6",
                    Vin = "1FM5K8D8XJGA43957",
                    Year = 2018
                }
            };

            var job5 = new JobListItemDto()
            {
                Id = "5",
                Code = "",
                Status = Job.JobStatus.Completed,
                CarInfo = new JobListItemDto.CarDto()
                {
                    Manufacturer = "Lexus",
                    Model = "RX450",
                    Vin = "1FM5K7D81JGA97674",
                    Year = 2019
                }
            };

            return await Task.FromResult(new List<JobListItemDto>() { job1, job2, job3, job4, job5 });
        }

        public async Task<RepairManual> GetRepairManualByIdAsync(string id)
        {
            RepairManual Headliner_RepairManual = new RepairManual()
            {
                Id = "6307dea4e92fe007aee9c57c",
                Name = "Quarter Panel",
                Category = RepairManual.RepairManualCategory.RemovalAndInstallation,
                Description = "",
                Materials = new List<Material>() {
                    new Material() {
                         Name = "Seam Sealer\r\nTA-2-B, 3M™ 08308, LORD Fusor® 803DTM\r\n",
                    },
                     new Material() {
                         Name = "Flexible Foam Repair\r\n3M™ 08463, LORD Fusor® 121\r\n",
                    }
                },
                SpecialTools = new List<SpecialTool>()
                {
                    new SpecialTool(){Name ="Resistance Spotwelding Equipment"},
                    new SpecialTool(){Name ="Spherical Cutter"},
                    new SpecialTool(){Name ="Hot Air Gun"},
                    new SpecialTool(){Name ="Air Body Saw"},
                    new SpecialTool(){Name ="8 mm Drill Bit"},
                    new SpecialTool(){Name ="MIG/MAG Welding Equipment"},
                    new SpecialTool(){Name ="Spot Weld Drill Bit"},
                    new SpecialTool(){Name ="Locking Pliers"}
                },
                DocumentUrl = "https://arastoragewtbg.blob.core.windows.net/repair-manuals/4c423020-f9db-46e4-b388-4d5d0dacfe2c.pdf",
                StepsGroups = new List<StepsGroup>()
            {
                new StepsGroup()
                {
                    Name = "Removal",
                    Messages = new List<Message>()
                    {
                        new Message(){
                            Type =Message.MessageType.Note,
                            Text = "Sectioning may not take place within 50 mm of restraints, door hinge, door striker or suspension mounting points."
                        },
                        new Message(){
                            Type =Message.MessageType.Note,
                            Text = "Factory welds may be substituted with resistance or metal inert gas (MIG) plug welds. Resistance welds may not be placed directly over original location. They must be placed adjacent to original location and match factory welds in quantity. Metal inert gas (MIG) plug welds must equal factory welds in both location and quantity."
                        },
                         new Message(){
                            Type =Message.MessageType.Note,
                            Text = "Adequately protect all adjacent areas against cutting, grinding and welding procedures."
                        },
                        new Message(){
                            Type =Message.MessageType.Note,
                            Text = "Liftgate and rear door removed for clarity."
                        },
                        new Message(){
                            Type =Message.MessageType.Note,
                            Text = "Left hand (LH) side shown, right hand (RH) side similar."
                        },
                    },
                    Steps = new List<ManualStep>()
                    {
                        new ManualStep()
                        {
                            Id = 1,
                            Title = "Sectioning Points:",
                            Description = "The quarter panel is constructed of mild steel and may be sectioned. Illustration provided as suggested sectioning points and is not all inclusive.",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>(){new ManualStepPhoto() { Name="", Url = "https://arastoragewtbg.blob.core.windows.net/repair-manual-photos/1.png" } }
                        },
                        new ManualStep()
                        {
                            Id = 2,
                            Title = "Depower the SRS.",
                            Description = "Refer to: Supplemental Restraint System (SRS) Depowering and Repowering (501-20B).",
                            Labels = new List<string> { "All vehicles" },
                            Messages = new List<Message>()
                            {
                                new Message(){Type = Message.MessageType.Note, Text = "RH side shown, LH side similar."}
                            },
                            SubSteps = new List<ManualStep>()
                            {
                                 new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Position aside the rear door weatherstrip."
                                 },
                                  new ManualStep()
                                 {
                                    Id = 2,
                                    Title = "Remove the bolt.",
                                    Messages = new List<Message>()
                                    {
                                        new Message() { Type = Message.MessageType.Note, Text = "Torque: 80 lb.in (9 Nm)" }
                                    }
                                 },
                                   new ManualStep()
                                 {
                                    Id = 3,
                                    Title = "Release the clips and the magnets."
                                 },
                                    new ManualStep()
                                 {
                                    Id = 4,
                                    Title = "Slide down and out and position asid e the C-pillar trim panel."
                                 }
                            },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "51b62a4081db4384b401ecc483efbe70", Url= "51b62a4081db4384b401ecc483efbe70.png"}
                            }
                        },
                        new ManualStep()
                        {
                            Id = 3,
                            Title = "Remove the retainers and the front grab handle",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "264f60289b6747498b630938f4635fa8", Url= "264f60289b6747498b630938f4635fa8.png"}
                            }
                        },
                        new ManualStep()
                        {
                            Id = 4,
                            Title = "Remove the sun visor",
                            Description = "On both sides.Remove the sun visor.",
                            Labels = new List<string> { "All vehicles" },
                            Messages = new List<Message>()
                                    {
                                        new Message() { Type = Message.MessageType.Note, Text = "RH side shown, LH side similar" }
                                    },
                            SubSteps = new List<ManualStep>()
                            {
                                 new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Remove the reta iners cover."
                                 },
                                  new ManualStep()
                                 {
                                    Id = 2,
                                    Title = "Remove the retainers.",
                                    Messages = new List<Message>()
                                    {
                                        new Message() { Type = Message.MessageType.Note, Text = "Torque: 80 lb.in (9 Nm)" }
                                    }
                                 },
                                   new ManualStep()
                                 {
                                    Id = 3,
                                    Title = "Seperate the sun visor."
                                 },
                                    new ManualStep()
                                 {
                                    Id = 4,
                                    Title = "If equipped. Disconnect and remove the sun visor."
                                 }
                            },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "95b1f9a6-7614-4374-898a-ceb54ce4c435", Url= "95b1f9a6-7614-4374-898a-ceb54ce4c435.png"}
                            }
                        },
                        new ManualStep()
                        {
                            Id = 5,
                            Title = "Remove the retainer and the sun visor clip.",
                            Labels = new List<string> { "All vehicles" },
                            Messages = new List<Message>()
                            {
                                new Message(){Type = Message.MessageType.Note, Text = "RH side shown, LH side similar."}
                            },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "6aa5d04ea08b4efeab92e71d0f497237", Url= "6aa5d04ea08b4efeab92e71d0f497237.png"}
                            }
                        },
                        new ManualStep()
                        {
                            Id = 6,
                            Title = "Position aside the headliner wire harness",
                            Description = "Position aside the headliner wire harness",
                            Labels = new List<string> { "All vehicles" },
                            SubSteps = new List<ManualStep>()
                            {
                                new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Disconnect the headliner wire harn ess electrical connector."
                                 },
                                new ManualStep()
                                 {
                                    Id = 2,
                                    Title = "Release the clips and position the wire harness aside."
                                 }
                            },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "bcf4634213d047ad895737bd5b16953b", Url= "bcf4634213d047ad895737bd5b16953b.png"}
                            }
                        },
                        new ManualStep()
                        {
                            Id = 7,
                            Title = "Position aside the washer hose.",
                            Description = "Position aside the washer hose.",
                            Labels = new List<string> { "All vehicles" },
                            SubSteps = new List<ManualStep>()
                            {
                                new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Disconnect the washer ho se coupling.",
                                    ReferencedManual = new RepairManual()
                                 },
                                new ManualStep()
                                 {
                                    Id = 2,
                                    Title = "Open the clips and position aside the washer hose."
                                 }
                            },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "379288e2fabc4f33910f774171273585", Url= "379288e2fabc4f33910f774171273585.png"}
                            }
                        },
                         new ManualStep()
                        {
                            Id = 8,
                            Title = "If equipped. Disconnect the rear view mirror electrical connector.",
                            Description= "If equipped. Disconnect the rear view mirror electrical connector.",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "4b67e2db1f2b43c6b7f696bd09c3e170", Url= "4b67e2db1f2b43c6b7f696bd09c3e170.png"}
                            }
                        },
                         new ManualStep()
                        {
                            Id = 9,
                            Title = "If equipped. Remove the rear view mirror cover.",
                            Description= "If equipped. Remove the rear view mirror cover.",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "27da974c811d4e699651351b48612f99", Url= "27da974c811d4e699651351b48612f99.png"}
                            }
                        },
                         new ManualStep()
                        {
                            Id = 10,
                            Title = "If equipped. Disconnect the rain sensor and rear view mirror electrical connectors.",
                            Description= "If equipped. Disconnect the rain sensor and rear view mirror electrical connectors.",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "ee31f2baf73f4fde9084dd9afa00ecca", Url= "ee31f2baf73f4fde9084dd9afa00ecca.png"}
                            }
                        },
                         new ManualStep()
                        {
                            Id = 11,
                            Title = "",
                            Description= "On both sides. Remove the second row coat hook.",
                            Labels = new List<string> { "Vehicles without roof opening panel" },
                            Messages = new List<Message>()
                            {
                                new Message(){Type = Message.MessageType.Note, Text = "RH side shown, LH side similar."}
                            },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "e4bac6ffffbe41fe9b53e312748b08ff", Url= "e4bac6ffffbe41fe9b53e312748b08ff.png"}
                            },
                            SubSteps = new List<ManualStep>()
                            {
                                new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Position the cover aside.",
                                    ReferencedManual = new RepairManual()
                                 },
                                new ManualStep()
                                 {
                                    Id = 2,
                                    Title = "Remove the retainer and the second row coat hook assembly."
                                 }
                            },
                        },
                         new ManualStep()
                        {
                            Id = 12,
                            Title = "",
                            Description= "On both sides. Remove the second row interior lamp.",
                            Labels = new List<string> { "Vehicles With: Roof Opening Panel" },
                            Messages = new List<Message>()
                            {
                                new Message(){Type = Message.MessageType.Note, Text = "RH side shown, LH side similar."}
                            },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "6fef442487f14dd4b0ecd7f1e857e2cd", Url= "6fef442487f14dd4b0ecd7f1e857e2cd.png"}
                            },
                            SubSteps = new List<ManualStep>()
                            {
                                new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Seperate the lamp assembly.",
                                    ReferencedManual = new RepairManual()
                                 },
                                new ManualStep()
                                 {
                                    Id = 2,
                                    Title = "Disconnect and remove the lamp assembly."
                                 }
                            },
                        },
                         new ManualStep()
                        {
                            Id = 13,
                            Title = "",
                            Description= "On both sides. Remove the ret ainers",
                            Labels = new List<string> { "Vehicles With: Roof Opening Panel" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "bf285239d44547e396bdab037f97e7c9", Url= "bf285239d44547e396bdab037f97e7c9.png"}
                            }
                        },
                         new ManualStep()
                        {
                            Id = 14,
                            Title = "Disconnect the D-pillar washer hose coupling.",
                            Description= "",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "0f1ec72b948c40ec8aba1da40f7e53e4", Url= "0f1ec72b948c40ec8aba1da40f7e53e4.png"}
                            },
                            ReferencedManual = new RepairManual()
                        },
                         new ManualStep()
                        {
                            Id = 15,
                            Title = "On both sides.",
                            Description= "Position the front seat in the full forward and reclined position.",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "9b9711c96bb641e0b23376e3865cf842", Url= "9b9711c96bb641e0b23376e3865cf842.png"}
                            },
                            SubSteps = new List<ManualStep>()
                            {
                                new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Position the seat in the full forward position.",
                                    ReferencedManual = new RepairManual()
                                 },
                                new ManualStep()
                                 {
                                    Id = 2,
                                    Title = "Position the seat in the full reclined position."
                                 }
                            }
                        },
                          new ManualStep()
                        {
                            Id = 16,
                            Title = "Release the clips and magnets and lower the headliner down.",
                            Description= "",
                            Labels = new List<string> { "Vehicles With: Roof Opening Panel" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "396e67a8-417d-4eab-894b-fd1459c77169", Url= "396e67a8-417d-4eab-894b-fd1459c77169.png"}
                            },
                            ReferencedManual = new RepairManual()
                        },
                           new ManualStep()
                        {
                            Id = 17,
                            Title = "Release the clips and magnets and lower the headliner down.",
                            Description= "",
                            Labels = new List<string> { "Vehicles without roof opening panel" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "4e953afb-4a45-4ab4-a9a0-f02dba851d4b", Url= "4e953afb-4a45-4ab4-a9a0-f02dba851d4b.png"}
                            }
                        },
                            new ManualStep()
                        {
                            Id = 18,
                            Title = "",
                            Description= "Remove the headliner.",
                            Labels = new List<string> { "All vehicles" },
                            Photos = new List<ManualStepPhoto>()
                            {
                                new ManualStepPhoto(){Name = "6907d84b146a4d3d81b2d6d343802f0c", Url= "6907d84b146a4d3d81b2d6d343802f0c.png"}
                            },
                            Messages = new List<Message>()
                            {
                                new Message(){Type = Message.MessageType.Note, Text = "To avoid damaging the headliner when folding the headliner wings, fold the headliner wings sothat the backing is ONLY touching the backing of the headliner. Never fold the headliner cloth-to-cloth or cloth-to-backing."},
                                new Message(){Type = Message.MessageType.Note, Text = "The headliner is made of new material that will allow for specific folding, flexing and rolling during removal through the liftgate opening."}
                            },
                        },

                    }
                },
                 new StepsGroup()
                {
                    Name = "Installation",
                    Messages = new List<Message>()
                    {
                        new Message(){Type = Message.MessageType.Note, Text = "Removal steps in this procedure may contain installation details."}
                    },
                    Steps = new List<ManualStep>()
                    {
                        new ManualStep()
                        {
                            Id = 1,
                            Title = "Remove the following items",
                            Labels = new List<string> { "All vehicles" },
                            Messages = new List<Message>()
                            {
                                new Message(){Type = Message.MessageType.Note, Text = "These steps are only necessary when installing a new component."},
                                new Message(){Type = Message.MessageType.Note, Text = @"When transferring the headliner harness to a new headliner note the location of each electrical connector
during removal and maintain those locations when transferring the harness. If equipped with noise cancellation the
microphone connectors are the same but are not interchangeable and must maintain their original locations for the
system to operate correctly."},
                                new Message(){Type = Message.MessageType.Note, Text = "Obtain the specified adhesive commercially. Depending on the headliner and optional wire harness(es), the purchase of multiple tubes is suggested."},
                            },
                            SubSteps = new List<ManualStep>()
                            {
                                 new ManualStep()
                                 {
                                    Id = 1,
                                    Title = "Using tape, mark the wire at the exit points for proper length from headliner-to-body/ roof connectors.",
                                    ReferencedManualId = 2
                                 }
                            }
                        },
                        new ManualStep()
                        {
                            Id = 2,
                            Title = "To install, reverse the removal procedure",
                            Labels = new List<string> { "All vehicles" },
                        },
                    }
                }
            }
            };
            return await Task.FromResult(Headliner_RepairManual);
        }
    }
}
