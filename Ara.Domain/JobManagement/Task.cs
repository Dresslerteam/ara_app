using Ara.Domain.Common;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.JobManagement
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SequenceNumber { get; set; }
        public List<Message> Messages { get; set; }
        public RepairManual RepairManual { get; set; }
        public int? RepairManualId { get; set; }
        public decimal Paint { get; set; }
        public decimal Labor { get; set; }
        public string PartNumber { get; set; }
        public decimal Quantity { get; set; }
        public TaskStatus Status { get; set; }
        public TaskResult Result { get; set; }
    }


    public enum TaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        OnHold = 3,
        Completed = 4
    }


    public class TaskResult
    {
        public List<StepResult> StepResults { get; set; }
        public class StepResult
        {
            public int? StepNumber { get; set; }
            public List<string> Photos { get; set; }

            public class Photo
            {
                public List<string> Labels { get; set; }
                public string Url { get; set; }
            }
        }
    }
}
