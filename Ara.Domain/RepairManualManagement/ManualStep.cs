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
        public List<Message> Messages { get; set; }
        public List<Photo> Photos { get; set; }
        public RepairManual ReferencedManual { get; set; }
        public int? ReferencedManualId { get; set; }
        public List<(StepReferencedDocType, PdfDoc)> Docs { get; set; }
        public enum StepReferencedDocType
        {
            Caution = 1,
            Procedure = 2
        }
    }
}
