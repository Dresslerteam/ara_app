using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ara.Domain.ApiClients.Interfaces
{
    internal interface IJobsClient
    {
        Task<List<JobListItemDto>> GetJobsAsync();
        Task<Job> GetJobByIdAsync(string id);
        Task<RepairManual> GetRepairManualByIdAsync(string id);
    }
}
