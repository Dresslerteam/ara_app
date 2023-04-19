using Ara.Domain.JobManagement;
using System;
using System.Collections.Generic;
using System.Text;
using static Ara.Domain.JobManagement.Job;

namespace Ara.Domain.ApiClients.Dtos
{
    public class JobListItemDto
    {
        public JobListItemDto(Job job)
        {
            Id = job.Id;
            RepairOrderNo = job.RepairOrderNo;
            ClaimNo = job.ClaimNo;
            EstimatorFullName = job.EstimatorFullName;
            CarOwner = job.CarOwner;
            Status = job.Status;
            NumberOfTasks = job.NumberOfTasks;
            NumberOfDoneTasks = job.NumberOfDoneTasks;
            PreliminaryEstimation = job.PreliminaryEstimation;
            PreliminaryScan = job.PreliminaryScan;
            CarInfo = new CarDto()
            {
                Vin = job.Car.Vin,
                Manufacturer = job.Car.Manufacturer,
                Model = job.Car.Model,
                Year = job.Car.Year
            };
            TotalNumberOfPhotos = job.TotalNumberOfPhotos;

        }

        public string Id { get; set; }
        public string RepairOrderNo { get; set; }
        public string ClaimNo { get; set; }
        public string EstimatorFullName { get; set; }
        public CarOwner CarOwner { get; set; }
        public JobStatus Status { get; set; }
        public int NumberOfTasks { get; set; }
        public int NumberOfDoneTasks { get; set; }
        public PdfDoc PreliminaryEstimation { get; set; }
        public PdfDoc PreliminaryScan { get; set; }
        public CarDto CarInfo { get; set; }
        public int TotalNumberOfPhotos { get; set; }

        public class CarDto
        {
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public string Vin { get; set; }
        }
    }
}
