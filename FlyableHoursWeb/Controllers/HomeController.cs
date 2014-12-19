using FlyableHours;
using FlyableHours.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlyableHoursWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new List<FlyingSite>());
        }

        [HttpGet]
        public ActionResult Index(string URL = "", string windspeed = "5", string minTemp = "-5", bool includeFog=true)
        {
            var site = new FlyingSite();
            Console.WriteLine("URL:" + URL);
            if (URL == "")
            {
                ViewBag.Title = "Search";
                ViewBag.Result = new List<FlyingSite>() { site };
                return View();
            }
            Console.WriteLine("Wind speed:" + windspeed);
            var parser = new YrXmlParser(URL);
            try
            {
                if (windspeed != null && windspeed.Length > 0)
                {
                    parser.MaxWindSpeed = float.Parse(windspeed, CultureInfo.InvariantCulture);
                }
                if (minTemp != null && minTemp.Length > 0)
                {
                    parser.MinTemperature = float.Parse(minTemp, CultureInfo.InvariantCulture);
                }
                parser.IncludeFoggyPeriods = includeFog;
            }
            catch (Exception)
            {
                //throw;
                ViewBag.Title = "Error";
                return View();
            }
            parser.findFlyableHours(site, out site);
            ViewBag.LocationName = site.ForecastLocationName;
            ViewBag.Result = new List<FlyingSite>(){site};
            //return new RedirectResult(Url.Action("Index") + "#Result");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}