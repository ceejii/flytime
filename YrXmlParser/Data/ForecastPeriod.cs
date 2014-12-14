using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyableHours.Data
{
    public class ForecastPeriod
    {
        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public String Forecast { get; set; }

        public String ForecastWindDirectionString { get; set; }

        public float ForecastWindDirectionDeg { get; set; }

        public float ForecastWindSpeed { get; set; }

        public float Precipitation { get; set; }

        public float ForecastTemperature { get; set; }
    }
}
