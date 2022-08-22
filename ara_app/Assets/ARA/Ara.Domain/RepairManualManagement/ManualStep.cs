using Ara.Domain.Common;
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
        public List<ManualStepPhoto> Photos { get; set; }
        public List<string> Labels { get; set; }
        public RepairManual ReferencedManual { get; set; }
        public int? ReferencedManualId { get; set; }
        public List<ManualStep> SubSteps { get; set; }
    }

    public class ManualStepPhoto
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

}
