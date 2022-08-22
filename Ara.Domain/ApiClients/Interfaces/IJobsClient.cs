using Ara.Domain.ApiClients.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ara.Domain.ApiClients.Interfaces
{
    internal interface IJobsClient
    {
        Task<List<JobListItemDto>> GetJobsAsync();
    }
}
