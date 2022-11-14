using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Repositories.Models
{
    public class TaskPhotoDAO
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool IsSynchronized { get; set; } = false;
        public DateTime? SynchronizedOn { get; set; }

    }
}
