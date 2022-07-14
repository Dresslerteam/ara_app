using Ara.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.RepairManualManagement
{
    public class ManualStep
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Message> Messages { get; set; }
        public List<string> Photos { get; set; }
        public List<string> Labels { get; set; }
    }
}
