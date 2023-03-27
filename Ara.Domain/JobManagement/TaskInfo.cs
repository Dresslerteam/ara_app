using Ara.Domain.Common;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Photo> Photos { get; set; } = new List<Photo>();

        public List<Photo> GetAllPhotos()
        {
            var taskPhotos = new List<Photo>();
            if (RepairManuals != null)
                foreach (var rep in RepairManuals)
                {
                    var photos = rep.Steps.Select(s => s.Photos).SelectMany(x => x).ToList();
                    taskPhotos.AddRange(photos);
                }
            taskPhotos.AddRange(Photos);
            return taskPhotos;
        }

        public enum TaskStatus
        {
            ToDo = 0,
            InProgress = 1,
            OnHold = 2,
            Completed = 3
        }
    }



    public class Photo
    {
        public int? TaskId { get; set; }
        public int? RepairManualId { get; set; }
        public int? StepId { get; set; }
        public string TaskName { get; set; }
        public string RepairManualName { get; set; }
        public string StepName { get; set; }

        public PhotoLabelType Label { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }

        public enum PhotoLabelType
        {
            Reinstall = 1,
            Replace,
            Repair,
            Other
        }
    }

    public class PdfDoc
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

}
