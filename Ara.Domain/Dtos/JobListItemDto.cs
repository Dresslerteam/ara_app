using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Dtos
{
    public class JobListItemDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int Status { get; set; }
        public decimal Progress { get; set; }
        public CarDto CarInfo { get; set; }

        public class CarDto
        {
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public string Year { get; set; }
            public string Vin { get; set; }
        }
    }
}
