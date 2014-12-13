using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyableHoursWeb.Models
{
    public class FlyingSite
    {
        public List<ForecastPeriod> ForecastPeriods { get; set; }

        public String ForecastUrl { get; set; }

        public String FlyingSiteInfoUrl { get; set; }

        public String FlyingSiteName { get; set; }

        public String ForecastLocationName { get; set; }

        public int PreferredWindDirectionMin { get; set; }

        public int PreferredWindDirectionMax { get; set; }

        public string TextForecast { get; set; }
    }
}