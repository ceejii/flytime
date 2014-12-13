using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyableHoursWeb.Models
{
    public class SlopeModel
    {
        public String Url{ get; set; }
        public String LocationName { get; set; }
        public IEnumerable<String> hourForecast { get; set; }
    }
}