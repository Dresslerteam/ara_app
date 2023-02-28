using Ara.Domain.Common;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.JobManagement
{
    public class TaskInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SequenceNumber { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public List<Message> Messages { get; set; }
        public List<RepairManual> RepairManuals { get; set; }
        public TaskStatus Status { get; set; }
        public List<Photo> Photos { get; set; }

        public enum TaskStatus
        {
            ToDo = 1,
            InProgress = 2,
            OnHold = 3,
            Completed = 4
        }
    }



    public class Photo
    {
        public int? TaskId { get; set; }
        public int? StepId { get; set; }
        public List<string> Labels { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class PdfDoc
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

}
