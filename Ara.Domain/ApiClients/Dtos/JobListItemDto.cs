using Ara.Domain.JobManagement;
using System;
using System.Collections.Generic;
using System.Text;
using static Ara.Domain.JobManagement.Job;

namespace Ara.Domain.ApiClients.Dtos
{
    public class JobListItemDto
    {
        public string Id { get; set; }
        public string RepairOrderNo { get; set; }
        public string ClaimNo { get; set; }
        public string EstimatorFullName { get; set; }
        public CarOwner CarOwner { get; set; }
        public JobStatus Status { get; set; }
        public int NumberOfTasks { get; set; }
        public int NumberOfDoneTasks { get; set; }
        public PdfDoc PreliminaryEstimation { get; set; }
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
