using FlyableHours;
using FlyableHours.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlyableHoursWeb.Controllers
{
    public class SlopesController : Controller
    {
        // GET: Slopes
        public ActionResult Index()
        {
            var slopes = new List<FlyingSite>();
            YrXmlParser parser = new YrXmlParser("http://www.yr.no/place/Sweden/Stockholm/V%C3%A4stberga/");
            parser.MaxWindSpeed = 20.7f;
            parser.MinTemperature = -20;

            var Bergshamra = new FlyingSite();
            Bergshamra.FlyingSiteName = "Bergshamra, Solna";
            Bergshamra.ForecastLocationName = "Bergshamra, Solna";
            Bergshamra.ForecastUrl = "http://www.yr.no/place/Sweden/Stockholm/Bergshamra~2722742/";
            Bergshamra.FlyingSiteInfoUrl = "http://hem.bredband.net/k_bergenfeldt/slopes/bergshamra/bergshamra.html";
            Bergshamra.PreferredWindDirectionMin = 200;
            Bergshamra.PreferredWindDirectionMax = 225;
            parser.Url = Bergshamra.ForecastUrl;
            parser.MinWindDirection = Bergshamra.PreferredWindDirectionMin;
            parser.MaxWindDirection = Bergshamra.PreferredWindDirectionMax;
            parser.findFlyableHours(Bergshamra, out Bergshamra);
            slopes.Add(Bergshamra);

            var Rotsunda = new FlyingSite();
            Rotsunda.FlyingSiteName = "Rotsunda, Gammalt grustag";
            Rotsunda.ForecastLocationName = "Rotebro, Sollentuna";
            Rotsunda.ForecastUrl = "http://www.yr.no/place/Sweden/Stockholm/Rotebro/";
            Rotsunda.FlyingSiteInfoUrl = "http://hem.bredband.net/k_bergenfeldt/slopes/rotsunda/rotsunda.html";
            Rotsunda.PreferredWindDirectionMin = 155;
            Rotsunda.PreferredWindDirectionMax = 205;
            parser.Url = Rotsunda.ForecastUrl;
            parser.MinWindDirection = Rotsunda.PreferredWindDirectionMin;
            parser.MaxWindDirection = Rotsunda.PreferredWindDirectionMax;
            parser.findFlyableHours(Rotsunda, out Rotsunda);
            slopes.Add(Rotsunda);

            var UpplandsVäsby = new FlyingSite();
            UpplandsVäsby.FlyingSiteName = "Upplands Väsby, gammalt grustag";
            UpplandsVäsby.ForecastLocationName = "Upplands Väsby";
            UpplandsVäsby.ForecastUrl = "http://www.yr.no/place/Sweden/Stockholm/Upplands_V%C3%A4sby/";
            UpplandsVäsby.FlyingSiteInfoUrl = "http://hem.bredband.net/k_bergenfeldt/slopes/upplandsvasby/uppl_vasby.html";
            UpplandsVäsby.PreferredWindDirectionMin = 65;
            UpplandsVäsby.PreferredWindDirectionMax = 160;
            parser.Url = UpplandsVäsby.ForecastUrl;
            parser.MinWindDirection = UpplandsVäsby.PreferredWindDirectionMin;
            parser.MaxWindDirection = UpplandsVäsby.PreferredWindDirectionMax;
            parser.findFlyableHours(UpplandsVäsby, out UpplandsVäsby);
            slopes.Add(UpplandsVäsby);

            return View(slopes);
        }
    }
}