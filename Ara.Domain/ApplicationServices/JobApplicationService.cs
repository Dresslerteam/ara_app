using Ara.Domain.Dtos;
using Ara.Domain.JobManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.ApplicationServices
{
    public class JobApplicationService
    {
        public List<JobListItemDto> GetJobs()
        {
            return new List<JobListItemDto>();
        }


        public Job GetJob(int id)
        {
            return new Job();
        }
    }
}
