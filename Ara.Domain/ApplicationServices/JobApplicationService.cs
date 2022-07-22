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
            return new Job()
            {
                Car = new Car()
                {
                    Manufacturer = "Ford",
                    Model = "Explorer",
                    Vin = "1FM5K8D8XJGA43957",
                    Year = "2018"
                },
                CarOwner = new CarOwner()
                {
                    FirstName = "T",
                    LastName = "Moss",
                    Mobile = "(406) 555-5555"
                },
                Tasks = new List<Task>()
                {
                    new Task()
                    {
                        Id = 1,
                        Title = "Headliner",
                        RepairManual = Headliner_RepairManual
                    }
                }
            };
        }

        public RepairManual Headliner_RepairManual = new RepairManual()
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
                        },
                        new ManualStep()
                        {
                            Id = 2,
                            Description = "On both sides",
                            Title = "RH side shown, LH side similar.",
                            Labels = new List<string> { "All vehicles" },
                            Messages = new List<Common.Message>()
                            {
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "RH side shown, LH side similar."}
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
                                    Messages = new List<Common.Message>()
                                    {
                                        new Common.Message() { Type = Common.Message.MessageType.Note, Text = "Torque: 80 lb.in (9 Nm)" }
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
                            Messages = new List<Common.Message>()
                                    {
                                        new Common.Message() { Type = Common.Message.MessageType.Note, Text = "RH side shown, LH side similar" }
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
                                    Messages = new List<Common.Message>()
                                    {
                                        new Common.Message() { Type = Common.Message.MessageType.Note, Text = "Torque: 80 lb.in (9 Nm)" }
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
                            Messages = new List<Common.Message>()
                            {
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "RH side shown, LH side similar."}
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
                            Messages = new List<Common.Message>()
                            {
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "RH side shown, LH side similar."}
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
                            Messages = new List<Common.Message>()
                            {
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "RH side shown, LH side similar."}
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
                            Messages = new List<Common.Message>()
                            {
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "To avoid damaging the headliner when folding the headliner wings, fold the headliner wings sothat the backing is ONLY touching the backing of the headliner. Never fold the headliner cloth-to-cloth or cloth-to-backing."},
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "The headliner is made of new material that will allow for specific folding, flexing and rolling during removal through the liftgate opening."}
                            },
                        },

                    }
                },
                 new StepsGroup()
                {
                    Name = "Installation",
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
                            Messages = new List<Common.Message>()
                            {
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "These steps are only necessary when installing a new component."},
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = @"When transferring the headliner harness to a new headliner note the location of each electrical connector
during removal and maintain those locations when transferring the harness. If equipped with noise cancellation the
microphone connectors are the same but are not interchangeable and must maintain their original locations for the
system to operate correctly."},
                                new Common.Message(){Type = Common.Message.MessageType.Note, Text = "Obtain the specified adhesive commercially. Depending on the headliner and optional wire harness(es), the purchase of multiple tubes is suggested."},
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
    }
}
