﻿using FlyableHours;
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
            return View();
        }

        [HttpPost]
        public ActionResult Index(string URL, string windspeed, string minTemp, bool includeFog)
        {
            Console.WriteLine("URL:" + URL);
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
                return View();
            }
            var site = new FlyingSite();
            parser.findFlyableHours(site, out site);
            Console.WriteLine(site.TextForecast);
            ViewBag.Result = site.TextForecast;// +"\r\n" + site.DebugInfo;
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