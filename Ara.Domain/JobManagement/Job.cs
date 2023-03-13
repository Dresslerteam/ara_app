using Ara.Domain.Common;
using Ara.Domain.RepairManualManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Ara.Domain.JobManagement.TaskInfo;

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

        public void UpdateTaskStatus(int taskId, TaskStatus newStatus)
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
            step.IsCompleted = true;
            return Result<object>.Ok(null);
        }

        public Result<(int RepairManualId, ManualStep Step)> GetNextStep(int taskId, int repairManualId, int stepId)
        {
            var task = this.Tasks.FirstOrDefault(t => t.Id == taskId);
            var repManual = task.RepairManuals.FirstOrDefault(r => r.Id == repairManualId);
            var step = repManual.Steps.FirstOrDefault(s => s.Id == stepId);
            var stepIndex = repManual.Steps.LastIndexOf(step);
            if (repManual.Steps.Count() > (stepIndex + 1))
            {
                var nextStep = repManual.Steps[stepIndex + 1];
            }
            else
            {
                var repairManualIndex = task.RepairManuals.LastIndexOf(repManual);
                if (task.RepairManuals.Count() > (repairManualIndex + 1))
                {
                    var nextRepairManual = task.RepairManuals[repairManualIndex + 1];
                    var nextStep = nextRepairManual.Steps.FirstOrDefault();
                    return Result<(int RepairManualId, ManualStep Step)>.Ok((nextRepairManual.Id, nextStep));
                }
            }

            return null;
        }

        public PdfDoc GetRepairManualDoc(int taskId, int repairManualId)
        {
            var task = this.Tasks.FirstOrDefault(t => t.Id == taskId);
            return task.RepairManuals.FirstOrDefault(r => r.Id == repairManualId).Document;
        }

        public void AssignPhoto(int taskId, int repairManualId, int stepId) { }

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
