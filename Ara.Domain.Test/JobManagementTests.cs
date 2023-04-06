using Ara.Domain.ApplicationServices;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ara.Domain.Test
{
    public class JobManagementTests
    {
        static Job __job = new Job()
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
                                           (ManualStep.StepReferencedDocType.Procedure, new PdfDoc(){Url = "1.png", Title = "1.png"}),
                                           (ManualStep.StepReferencedDocType.Caution, new PdfDoc(){Url = "1.png", Title = "1.png"}),
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

        [Fact]
        public async void GetJobs()
        {
            var jobService = new JobApplicationService();
            var jobs = await jobService.GetJobsAsync();
            Assert.True(true);
        }

        [Fact]
        public async void GetJobById()
        {
            var jobService = new JobApplicationService();
            var jobs = await jobService.GetJobDetailsAsync("1");
            Assert.True(true);
        }

        [Fact]
        public async void ChangeTaskStatus()
        {
            var jobService = new JobApplicationService();
            var job = await jobService.GetJobDetailsAsync("1");
            Assert.True(job.NumberOfDoneTasks == 0);

            job.ChangeTaskStatus(1, JobManagement.TaskInfo.TaskStatus.InProgress);
            job.ChangeTaskStatus(1, JobManagement.TaskInfo.TaskStatus.Completed);
            job.ChangeTaskStatus(2, JobManagement.TaskInfo.TaskStatus.Completed);

            //job.Tasks.FirstOrDefault().RepairManuals.FirstOrDefault().Steps.FirstOrDefault().ReferencedDocs.FirstOrDefault().Type == 

            Assert.True(job.Status == JobManagement.Job.JobStatus.Completed);

            var jobs = await jobService.GetJobsAsync();
            var jobItem = jobs.FirstOrDefault(f => f.Id == job.Id);
            Assert.True(jobItem.NumberOfDoneTasks == job.NumberOfDoneTasks);
            Assert.True(jobItem.Status == JobManagement.Job.JobStatus.Completed);


        }


        [Fact]
        public async void GetStep()
        {
            var jobService = new JobApplicationService();
            var job = await jobService.GetJobDetailsAsync("1");
            job.ChangeTaskStatus(1, JobManagement.TaskInfo.TaskStatus.InProgress);
            job.ChangeTaskStatus(1, JobManagement.TaskInfo.TaskStatus.Completed);
            job.ChangeTaskStatus(2, JobManagement.TaskInfo.TaskStatus.Completed);

            var a = job.Tasks.FirstOrDefault(t => t.Id == 1).RepairManuals.FirstOrDefault(r => r.Id == 2).Steps.FirstOrDefault(s => s.Id == 19).ReferencedDocs.FirstOrDefault();

            Assert.True(a.Type == RepairManualManagement.ManualStep.StepReferencedDocType.Procedure);

            Assert.True(job.Status == JobManagement.Job.JobStatus.Completed);

            var jobs = await jobService.GetJobsAsync();
            var jobItem = jobs.FirstOrDefault(f => f.Id == job.Id);
            Assert.True(jobItem.Status == JobManagement.Job.JobStatus.Completed);


        }

        [Fact]
        public async void CompleteStep()
        {
            var job = __job;

            Assert.True(job.Status == JobManagement.Job.JobStatus.ToDo);
            var task = job.Tasks.FirstOrDefault(t => t.Id == 1);
            var repManual = task.RepairManuals.FirstOrDefault(r => r.Id == 1);

            var step1 = repManual.Steps.FirstOrDefault();
            Assert.True(step1.PhotoRequired);
            var result1 = job.CompleteStep(task.Id, repManual.Id, step1.Id);
            Assert.True(result1.Status == Common.ResultStatus.Failure);
            Assert.True(result1.ErrorCode == Common.ErrorCode.StepPhotoRequired);

            //Pat took Photo, named it like
            var photoName = DateTime.Now.ToString("yyyy_dd_M__HH_mm_ss_ms") + ".png";

            job.AssignPhotoToStep(task.Id, repManual.Id, step1.Id, photoName, Photo.PhotoLabelType.Repair);
            var result1_1 = job.CompleteStep(task.Id, repManual.Id, step1.Id);
            Assert.True(result1_1.Status == Common.ResultStatus.Success);


            var step2 = repManual.Steps.FirstOrDefault(s => s.Id == 2);

            var step2ByNextMethod = job.GetNextStep(task.Id, repManual.Id, step1.Id).Payload.Step;

            Assert.True(step2 == step2ByNextMethod);

            var result2 = job.CompleteStep(task.Id, repManual.Id, step2ByNextMethod.Id);
            Assert.True(result2.Status == Common.ResultStatus.Success);

            var step3ByNextMethod = job.GetNextStep(task.Id, repManual.Id, step2ByNextMethod.Id);
            Assert.True(step3ByNextMethod.Payload.RepairManualId == 2);


        }


        [Fact]
        public async void GalleryTest()
        {
            var job = __job;

            Assert.True(job.Status == JobManagement.Job.JobStatus.ToDo);
            var task = job.Tasks.FirstOrDefault(t => t.Id == 1);
            var repManual = task.RepairManuals.FirstOrDefault(r => r.Id == 1);

            var step1 = repManual.Steps.FirstOrDefault();
            Assert.True(step1.PhotoRequired);
            var result1 = job.CompleteStep(task.Id, repManual.Id, step1.Id);
            Assert.True(result1.Status == Common.ResultStatus.Failure);
            Assert.True(result1.ErrorCode == Common.ErrorCode.StepPhotoRequired);

            //Pat took Photo, named it like
            var photoName = DateTime.Now.ToString("yyyy_dd_M__HH_mm_ss_ms") + ".png";

            job.AssignPhotoToStep(task.Id, repManual.Id, step1.Id, photoName, Photo.PhotoLabelType.Repair);

            var gallry1 = job.GetJobGallery();
            
            var result1_1 = job.CompleteStep(task.Id, repManual.Id, step1.Id);
            Assert.True(result1_1.Status == Common.ResultStatus.Success);


            var step2 = repManual.Steps.FirstOrDefault(s => s.Id == 2);

            var step2ByNextMethod = job.GetNextStep(task.Id, repManual.Id, step1.Id).Payload.Step;

            Assert.True(step2 == step2ByNextMethod);

            var result2 = job.CompleteStep(task.Id, repManual.Id, step2ByNextMethod.Id);
            Assert.True(result2.Status == Common.ResultStatus.Success);

            var step3ByNextMethod = job.GetNextStep(task.Id, repManual.Id, step2ByNextMethod.Id);
            Assert.True(step3ByNextMethod.Payload.RepairManualId == 2);


        }

    }
}
