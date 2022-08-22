using System;
using System.Collections.Generic;
using System.Text;
using static Ara.Domain.JobManagement.Job;

namespace Ara.Domain.ApiClients.Dtos
{
    public class JobListItemDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public JobStatus Status { get; set; }
        public decimal Progress { get; set; }
        public CarDto CarInfo { get; set; }

        public class CarDto
        {
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public string Vin { get; set; }
        }
    }
}
