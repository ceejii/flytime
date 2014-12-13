using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyableHoursWeb.Models
{
    public class ForecastPeriod
    {
        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public String Forecast { get; set; }

        public String ForecastWindDirection { get; set; }

        public float ForecastWindSpeed { get; set; }
    }
}
