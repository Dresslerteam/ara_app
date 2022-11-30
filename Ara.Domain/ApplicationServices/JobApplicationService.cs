using Ara.Domain.ApiClients;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Interfaces;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ara.Domain.ApplicationServices
{
    public class JobApplicationService
    {
        private readonly IJobsClient _jobsClient;
        public JobApplicationService()
        {
            _jobsClient = new MockJobsClient();
        }
        public async Task<List<JobListItemDto>> GetJobsAsync()
        {
            var jobs = await _jobsClient.GetJobsAsync();
            return jobs;
        }

        public async Task<Job> GetJobDetailsAsync(string id)
        {
            var job = await _jobsClient.GetJobByIdAsync(id);
            return job;
        }

        public async Task<RepairManual> GetRepairManualAsync(string id)
        {
            var repairManual = await _jobsClient.GetRepairManualByIdAsync(id);
            return repairManual;
        }

        public void SubbmitTaskResult()
        {

        }

       


    }
}
