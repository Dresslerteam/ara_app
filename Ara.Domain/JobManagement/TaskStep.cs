﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.JobManagement
{
    public class TaskStep
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Message> Messages { get; set; }
        public List<string> Photos { get; set; }
    }
}
