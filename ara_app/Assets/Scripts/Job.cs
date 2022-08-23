using System.Collections.Generic;

namespace ARA.Frontend
{
    [System.Serializable]
    public class Job
    {
        public string jobNumber;
        public string autoYear = "2008";
        public string manufacturer = "Ford";
        public string autoModel = "Ranger";
        public string vin;
        public List<TaskInfoModel> tasks;

        public string GetVehicleTitleString()
        {
            return (autoYear + " " + manufacturer + " " + autoModel);
        }
    }
}