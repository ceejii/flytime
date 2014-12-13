using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;
using System.Configuration;

namespace FlyableHours
{
    public class YrXmlParser
    {
        public String Url { get; set; }
        public String XmlFileName { get; set; }
        private static Dictionary<String,XmlSnapshot> xmlCache = new Dictionary<String,XmlSnapshot>();
        private TimeSpan CacheTime = new TimeSpan(0, -10, 0);
        private bool DEBUG = false;

        public bool IncludeFoggyPeriods { get; set; }
        public float MaxWindSpeed { get; set; }
        public float MinTemperature { get; set; }
        public float MinWindDirection { get; set; }
        public float MaxWindDirection { get; set; }

        static void Main(string[] args)
        {
            var parser = new YrXmlParser(args[0]);
            //parser.notify(parser.findFlyableHours());
            Console.WriteLine(parser.findFlyableHours());
            Console.WriteLine(parser.findFlyableHours());
            Console.ReadKey();
        }
    

        public YrXmlParser (String Url, String XmlFileName = "varsel_time_for_time.xml")
	    {
            this.Url = Url;
            this.XmlFileName = XmlFileName;
            this.MaxWindSpeed = float.Parse(ConfigurationManager.AppSettings["maxFlyableWindSpeed"]);
            this.MinTemperature = float.Parse(ConfigurationManager.AppSettings["flyableTemperatureMin"]);
            this.MinWindDirection = float.Parse(ConfigurationManager.AppSettings["flyableWindDirectionMin"]);
            this.MaxWindDirection = float.Parse(ConfigurationManager.AppSettings["flyableWindDirectionMax"]);
            this.IncludeFoggyPeriods = bool.Parse(ConfigurationManager.AppSettings["includeFoggyPeriods"]);
        }

        public string findFlyableHours()
        {
            int flyableHours = 0;
            var cacheResult = new StringBuilder();
            CleanupCache(cacheResult);
            XmlDocument xmlDoc = new XmlDocument();
            if (xmlCache.ContainsKey(Url))
            {
                cacheResult.AppendLine("Cache entry found for URL " + Url + XmlFileName);
                var tempXD = xmlCache[Url];
                    var age = (DateTime.Now - tempXD.TimeStamp);
                    cacheResult.AppendLine("Using cached version from " + age.Minutes + " minutes " + age.Seconds + " seconds ago.");
                    xmlDoc = tempXD.XmlDoc;
            }
            else
            {
                cacheResult.AppendLine("No match in cache.");
                xmlCache.Remove(Url);
                xmlDoc = new XmlDocument();
                try
                {
                    LoadPathToXmlDocument(cacheResult, xmlDoc);
                }
                catch (Exception e)
                {
                    return "Something went wrong getting the xml forecast. Url used: " + Url + XmlFileName /*+ " " + e.Message + " " + e.Source + " " + e.StackTrace*/;
                    //throw;
                }
            }

#region Location
            StringBuilder output = new StringBuilder();
            XmlNodeList location = xmlDoc.GetElementsByTagName("location");
            String place = "";
            String country = "";
            if (location.Count > 0)
            {
                XmlNodeList locationChildren = location[0].ChildNodes;
                foreach (XmlNode child in locationChildren)
	                {
		                 if(child.Name == "name") {
                             place = child.InnerText;
                         }
		                 if(child.Name == "country") {
                             country = child.InnerText;
                         }
	                }
                //output.AppendLine("Forecasted flying hours for: " + place + ", " + country);
            }
            else
            {
                output.AppendLine("Unexpected xml structure for location.");
            }
#endregion Location

#region Last update
            XmlNodeList lastUpdate = xmlDoc.GetElementsByTagName("lastupdate");
            DateTime lastUpdateTime = DateTime.Now;
            if (lastUpdate.Count == 1)
            {
                String luString = lastUpdate[0].InnerText;
                lastUpdateTime = ParseYrDateString(luString);
                //output.AppendLine("Forecast as of: " + lastUpdateTime.ToString());
            }
            else
            {
                output.AppendLine("Unexpected xml structure for last update.");
            }
#endregion Last update

#region Next update
            XmlNodeList nextUpdate = xmlDoc.GetElementsByTagName("nextupdate");
            DateTime nextUpdateTime = DateTime.MaxValue;
            if (nextUpdate.Count == 1)
            {
                String nuString = nextUpdate[0].InnerText;
                nextUpdateTime = ParseYrDateString(nuString);
                //output.AppendLine("Next forecast expected at: " + nextUpdateTime.ToString());
            }
            else
            {
                output.AppendLine("Unexpected xml structure for next update.");
            }
            output.AppendLine("Forecast as of " + lastUpdateTime.ToString() + " for " + place + ", " + country + ". Next forecast at " + nextUpdateTime);
#endregion Next update

#region Sun
            XmlNodeList sun = xmlDoc.GetElementsByTagName("sun");
            DateTime sunRise = DateTime.Now;
            DateTime sunSet = DateTime.Now;
            //Check if the sun sets and rises. (In the far north and far south of the world it doesn't always rise/set every day depending on season.)
            bool polarNight = getAttributeValue(sun[0],"never_rise") != null;
            bool polarDay = getAttributeValue(sun[0], "never_set") != null;
            if (polarNight) 
            { 
                //Do nothing, no sun hours to parse.
                output.AppendLine("Polar Night at this location. No periods available.");
            } else if (polarDay){
                //Do nothing. All hours parseable.
                output.AppendLine("Polar Day at this location. All periods available.");
            }
            else if (sun.Count == 1)
            {
                sunRise = ParseYrDateString(getAttributeValue(sun[0], "rise"));
                sunSet = ParseYrDateString(getAttributeValue(sun[0], "set"));
                output.AppendLine("Sunrise: " + sunRise + " Sunset: " + sunSet);
            }
            else
            {
                output.AppendLine("Unexpected xml structure for sun set/rise.");
            }
#endregion Sun

#region Hours
            XmlNodeList periods = xmlDoc.GetElementsByTagName("time");
            foreach (XmlNode period in periods)
            {
                XmlNodeList hourElements = period.ChildNodes;
                float precipitation = 200.0f;
                float windSpeed = 200.0f;
                float windDirection = 0.0f;
                string windDirectionString = "";
                float temperature = 200.0f;
                String symbolString = "";
                bool fog = false;

                foreach (XmlNode hourChild in hourElements)
                {
                    if (hourChild.Name == "precipitation")
                    {
                        precipitation = float.Parse(getAttributeValue(hourChild,"value"), CultureInfo.InvariantCulture);
                    }
                    if (hourChild.Name == "windSpeed")
                    {
                        windSpeed = float.Parse(getAttributeValue(hourChild, "mps"), CultureInfo.InvariantCulture);
                    }
                    if (hourChild.Name == "windDirection")
                    {
                        windDirection = float.Parse(getAttributeValue(hourChild, "deg"), CultureInfo.InvariantCulture);
                        windDirectionString = getAttributeValue(hourChild, "code");
                    }
                    if (hourChild.Name == "temperature")
                    {
                        temperature = float.Parse(getAttributeValue(hourChild,"value"), CultureInfo.InvariantCulture);
                    }
                    if (hourChild.Name == "symbol")
                    {
                        int symbol = int.Parse(getAttributeValue(hourChild, "number"), CultureInfo.InvariantCulture);
                        switch (symbol)
                        {
                            case    1:
                                symbolString = "sunny";
                                break;
                            case 2:
                                symbolString = "fair";
                                break;
                            case 3:
                                symbolString = "partly cloudy";
                                break;
                            case 4:
                                symbolString = "cloudy";
                                break;
                            case 15:
                                symbolString = "fog";
                                fog = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
                //output.AppendLine("min " + MinWindDirection + " max " + MaxWindDirection + " value " + windDirection);
                if (precipitation == 0.0f 
                    && windSpeed <= MaxWindSpeed
                    && temperature >= MinTemperature
                    && !polarNight
                    && (polarDay || ParseYrDateString(getAttributeValue(period, "to")).TimeOfDay > sunRise.TimeOfDay)
                    && (polarDay || ParseYrDateString(getAttributeValue(period, "from")).TimeOfDay < sunSet.TimeOfDay)
                    && (IncludeFoggyPeriods || (!IncludeFoggyPeriods && !fog))
                    && windDirection <= MaxWindDirection
                    && windDirection >= MinWindDirection
                    )
                {
                    //TODO: Parse into typed object instead.
                    var from = ParseYrDateString(getAttributeValue(period, "from"));
                    var to = ParseYrDateString(getAttributeValue(period, "to"));
                    output.AppendLine(from.DayOfWeek + " " + from.ToString("d MMM HH:mm" + "-") + to.ToString("HH:mm") + " rain:" + precipitation + " windspeed:" + windSpeed + " winddirection: " + windDirectionString + "(" + Math.Round(windDirection,0) + ") temperature:" + temperature + " " + symbolString);
                    flyableHours++;
                }
            }
            output.AppendLine("Flyable periods: " + flyableHours);

#endregion

#region Credit
            XmlNodeList credit = xmlDoc.GetElementsByTagName("credit");
            if (credit.Count == 1)
            {
                XmlNodeList creditChildren = credit[0].ChildNodes;
                foreach (XmlNode child in creditChildren)
                {
                    if (child.Name == "link")
                    {
                        output.AppendLine();
                        output.AppendLine(getAttributeValue(child, "text"));
                        output.AppendLine(getAttributeValue(child, "url"));
                    }
                }
            }
            else
            {
                output.AppendLine("Unexpected xml structure for credit.");
            }
#endregion Credit
            if (DEBUG)
            {
                output.AppendLine("");
                output.AppendLine("- - - Cache info - - -");
                output.AppendLine("");
                output.Append(cacheResult);
                output.AppendLine("");
                output.AppendLine("- - - Parameter info - - -");
                output.AppendLine("");
                output.AppendLine("Min temp: " + this.MinTemperature);
                output.AppendLine("Max wind speed: " + this.MaxWindSpeed);
                output.AppendLine("Min wind direction: " + this.MinWindDirection);
                output.AppendLine("Max wind direction: " + this.MaxWindDirection);
                output.AppendLine("Include fog: " + this.IncludeFoggyPeriods);
                output.AppendLine("Url: " + this.Url);
                output.AppendLine("Xml file: " + this.XmlFileName);
            }

            return output.ToString();
        }

        private void CleanupCache(StringBuilder result)
        {
            var cacheThresholdAge = DateTime.Now.Add(CacheTime);
            //(tempXD.TimeStamp > DateTime.Now.Add(new TimeSpan(0,-10,0)))
            foreach (var item in xmlCache.Where(item => (item.Value.TimeStamp < cacheThresholdAge)).ToList())
            {
                result.AppendLine("Removing old cache entry: " + item.Key + " of age " + (DateTime.Now - item.Value.TimeStamp));
                xmlCache.Remove(item.Key);
            }
        }

        private void LoadPathToXmlDocument(StringBuilder output, XmlDocument xmlDoc)
        {
            output.AppendLine("Getting xml from yr.");
            xmlDoc.Load(Url + XmlFileName); // Load the XML document from the specified file
            output.AppendLine("Adding retrieved xml to cache.");
            xmlCache.Add(Url, new XmlSnapshot(DateTime.Now, xmlDoc));
        }

        public void notify(string output)
        {
            //TODO: Separate notifications from parser
            sendEmail(output);
            logToConsole(output);
        }

        private void logToConsole(string output)
        {
            Console.WriteLine(output);
        }

        public void sendEmail(string output)
        {
            //TODO: Separate mail service from parser.
            new GmailMailService().senedGmailMail(output.ToString());
        }

#region Helpers
        private static DateTime ParseYrDateString(String luString)
        {
            DateTime lastUpdateTime = DateTime.ParseExact(luString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            return lastUpdateTime;
        }

        private static String getAttributeValue(XmlNode node, String attributeName)
        {
            XmlAttributeCollection attributes = node.Attributes;
            var attribute = attributes.GetNamedItem(attributeName);
            if (attribute == null)
            {
                return null;
            }
            return attribute.Value;
        }
#endregion
    }
}