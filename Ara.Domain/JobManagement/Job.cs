using Ara.Domain.Common;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Ara.Domain.JobManagement.Photo;
using static Ara.Domain.JobManagement.TaskInfo;

namespace Ara.Domain.JobManagement
{
    public class Job
    {
        public string Id { get; set; }
        public string RepairOrderNo { get; set; }
        public string ClaimNo { get; set; }
        public int NumberOfTasks => Tasks.Count();
        public int NumberOfDoneTasks => Tasks.Count(t => t.Status == TaskStatus.Completed);
        public JobStatus Status { get; set; }
        public Car Car { get; set; }
        public CarOwner CarOwner { get; set; }
        public List<TaskInfo> Tasks { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EstimatorFullName { get; set; }
        public PdfDoc PreliminaryEstimation { get; set; }
        public PdfDoc PreliminaryScan { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();

        public List<Photo> GetAllPhotos()
        {
            var photos = Tasks != null ? Tasks.Select(t => t.GetAllPhotos()).SelectMany(p => p).ToList() : new List<Photo>();
            photos.AddRange(Photos);
            return photos;
        }

        public List<(string Date, string TaskName, IEnumerable<Photo> Photos)> GetJobGallery()
        {
            var allPhotos = GetAllPhotos();

            var groupedData = (from ph in allPhotos
                               group ph by new { Date = ph.CreatedOn.Date.ToString(), ph.TaskId, ph.TaskName } into g
                               select new { Date = g.Key.Date, TaskName = g.Key.TaskName, Photos = g.Select(x => x) })
                              .Select(c => (c.Date, c.TaskName, c.Photos)).ToList();

            return groupedData;
        }

        public void ChangeTaskStatus(int taskId, TaskStatus newStatus)
        {
            var task = this.Tasks.FirstOrDefault(t => t.Id == taskId);
            task.Status = newStatus;
            this.updateJobStatus();
        }
        public Result<object> CompleteStep(int taskId, int repairManualId, int stepId)
        {
            var task = this.Tasks.FirstOrDefault(t => t.Id == taskId);
            var repManual = task.RepairManuals.FirstOrDefault(r => r.Id == repairManualId);
            var step = repManual.Steps.FirstOrDefault(s => s.Id == stepId);
            if (step.PhotoRequired && (step.Photos == null || step.Photos.Count() == 0))
            {
                return Result<object>.Failed(ErrorCode.StepPhotoRequired);
            }
            step.IsCompleted = true;
            return Result<object>.Ok(null);
        }

        public Result<(RepairManual RepairManual, ManualStep Step)> GetNextStep(int taskId, int repairManualId, int stepId)
        {
            var task = this.Tasks.FirstOrDefault(t => t.Id == taskId);
            var repManual = task.RepairManuals.FirstOrDefault(r => r.Id == repairManualId);
            var step = repManual.Steps.FirstOrDefault(s => s.Id == stepId);
            var stepIndex = repManual.Steps.LastIndexOf(step);
            if (repManual.Steps.Count() > (stepIndex + 1))
            {
                var nextStep = repManual.Steps[stepIndex + 1];
                return Result<(RepairManual RepairManual, ManualStep Step)>.Ok((repManual, nextStep));
            }
            else
            {
                var repairManualIndex = task.RepairManuals.LastIndexOf(repManual);
                if (task.RepairManuals.Count() > (repairManualIndex + 1))
                {
                    var nextRepairManual = task.RepairManuals[repairManualIndex + 1];
                    var nextStep = nextRepairManual.Steps.FirstOrDefault();
                    return Result<(RepairManual RepairManual, ManualStep Step)>.Ok((nextRepairManual, nextStep));
                }
            }

            return null;
        }

        public PdfDoc GetRepairManualDoc(int taskId, int repairManualId)
        {
            var task = this.Tasks.FirstOrDefault(t => t.Id == taskId);
            return task.RepairManuals.FirstOrDefault(r => r.Id == repairManualId).Document;
        }

        public void AssignPhotoToStep(int taskId, int repairManualId, int stepId, string photoUrl, PhotoLabelType label)
        {
            var task = this.Tasks.FirstOrDefault(t => t.Id == taskId);
            var repManual = task.RepairManuals.FirstOrDefault(r => r.Id == repairManualId);
            var step = repManual.Steps.FirstOrDefault(s => s.Id == stepId);

            if (step.Photos == null)
                step.Photos = new List<Photo>();

            step.Photos.Add(new Photo() { Url = photoUrl, CreatedOn = DateTime.Now, Label = label, TaskId = taskId, StepId = stepId, TaskName = task.Title, StepName = step.Title, RepairManualId = repairManualId, RepairManualName = repManual.Name });
        }

        private void updateJobStatus()
        {
            if (this.Tasks.All(t => t.Status == TaskStatus.Completed))
                this.Status = JobStatus.Completed;
            else
            {
                if (this.Tasks.All(t => t.Status == TaskStatus.ToDo))
                {
                    this.Status = JobStatus.ToDo;
                }
                else
                {
                    this.Status = JobStatus.InProgress;
                }
            }
        }

        public enum JobStatus
        {
            ToDo = 1,
            InProgress = 2,
            Completed = 3
        }

    }
}
