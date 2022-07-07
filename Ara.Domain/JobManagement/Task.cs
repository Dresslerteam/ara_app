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
        public List<string> SepcialTools { get; set; }
        public List<Matraial> Materials { get; set; }
        public List<Message> Messages { get; set; }
        public string RepairManualUrl { get; set; }
        public decimal Paint { get; set; }
        public decimal Labor { get; set; }
        public string PartNumber { get; set; }
        public decimal Quantity { get; set; }
        public List<StepsGroup> StepsGroups { get; set; }

        public TaskStatus Status { get; set; }

        public class StepsGroup
        {
            public int SequenceNumber { get; set; }
            public string Name { get; set; }
            public List<TaskStep> Steps { get; set; }

        }
    }


    public enum TaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        OnHold = 3,
        Completed = 4
    }
}
