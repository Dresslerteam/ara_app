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
    }
}
