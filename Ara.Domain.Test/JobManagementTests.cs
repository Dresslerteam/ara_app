using Ara.Domain.ApplicationServices;
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
    }
}
