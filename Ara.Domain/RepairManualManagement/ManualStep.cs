using Ara.Domain.Common;
using Ara.Domain.JobManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.RepairManualManagement
{
    public class ManualStep
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StepImage Image { get; set; }
        public List<Message> Messages { get; set; }
        public List<Photo> Photos { get; set; }
        public bool PhotoRequired { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public List<(StepReferencedDocType, PdfDoc)> ReferencedDocs { get; set; }
        public enum StepReferencedDocType
        {
            Caution = 1,
            Procedure = 2
        }

        public class StepImage
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
    }
}
