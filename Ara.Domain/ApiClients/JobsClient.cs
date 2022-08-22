using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApiClients.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ara.Domain.ApiClients
{
    internal class JobsClient : IJobsClient
    {
        static readonly HttpClient _client = new HttpClient();
        public JobsClient()
        {

        }
        public async Task<List<JobListItemDto>> GetJobsAsync()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await _client.GetAsync("http://api.ara.anyml.ai/api/Jobs");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var jobsList = JsonConvert.DeserializeObject<List<JobListItemDto>>(responseBody);

                return jobsList;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }
    }
}
