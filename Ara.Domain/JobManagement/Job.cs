using System;
using System.Collections.Generic;

namespace Ara.Domain.JobManagement
{
    public class Job
    {
        public string Id { get; set; }
        public string RepairOrderNo { get; set; }
        public string ClaimNo { get; set; }
        public int NumberOfTasks { get; set; }
        public int NumberOfDoneTasks { get; set; }
        public JobStatus Status { get; set; }
        public Car Car { get; set; }
        public CarOwner CarOwner { get; set; }
        public List<TaskInfo> Tasks { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EstimatorFullName { get; set; }
        public PdfDoc PreliminaryEstimation { get; set; }
        public PdfDoc PreliminaryScan { get; set; }
        public List<Photo> Photos { get; set; }

        public void UpdateTaskStatus() { }
        public void UpdateStepStatus() { }
        public void CompleteStep() { }
        public void AssignPhoto() { }

        public enum JobStatus
        {
            ToDo = 1,
            InProgress = 2,
            Completed = 3
        }

    }
}
