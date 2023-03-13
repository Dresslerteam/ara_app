using Ara.Domain.Common;
using Ara.Domain.JobManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.RepairManualManagement
{
    public class RepairManual
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PdfDoc Document { get; set; }
        public List<SpecialTool> SpecialTools { get; set; }
        public List<Material> Materials { get; set; }
        public List<ManualStep> Steps { get; set; }
        public RepairManualCategory Category { get; set; }

        public enum RepairManualCategory
        {
            RemovalAndInstallation = 1,
            GeneralProcedure = 2,
            DescriptionAndOperation = 3
        }
    }


    public enum StepStatus
    {
        ToDo = 1,
        InProgress = 2,
        Completed = 3
    }
}
