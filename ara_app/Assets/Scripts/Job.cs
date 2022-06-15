using System.Collections.Generic;

namespace ARA.Frontend
{
    [System.Serializable]
    public class Job
    {
        public string jobNumber;
        public string vehicleTitle;
        public string completeness;
        public string vin;
        public List<Task> tasks;
    }
}