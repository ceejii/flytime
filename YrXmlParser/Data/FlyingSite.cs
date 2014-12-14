using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyableHours.Data
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

        public string DebugInfo { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public DateTime NextUpdateTime { get; set; }

        public DateTime SunRise { get; set; }

        public DateTime SunSet { get; set; }

        public string Credits { get; set; }

        public FlyingSite()
        {
            PreferredWindDirectionMin = 0;
            PreferredWindDirectionMax = 360;
        }
    }
}