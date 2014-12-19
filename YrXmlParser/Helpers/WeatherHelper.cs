using System;

namespace FlyableHours.Helpers
{
    public class WeatherHelper
    {

        public static string CalculateWindChill(double temperature, float windSpeed)
        {
            if (temperature > 10)
            {
                return "";
            }
            if (windSpeed < 2.0f)
            {
                return "";
            }
            double result = Math.Round(13.126665f + 0.6215f * temperature - 13.924748f * Math.Pow(windSpeed, 0.16f) + 0.4875195f * temperature * Math.Pow(windSpeed, 0.16f), 0);
            return result.ToString();
        }
    }
}
