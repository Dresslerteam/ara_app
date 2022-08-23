using System;
using System.Collections.Generic;

namespace Ara.Domain.JobManagement
{
    public class Job
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public CarOwner CarOwner { get; set; }
        public string Title { get; set; }
        public List<TaskInfo> Tasks { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public List<string> InitialPhotos { get; set; }
        public JobStatus Status { get; set; }

        public enum JobStatus
        {
            ToDo = 1,
            InProgress = 2,
            Completed = 3
        }

    }
}
