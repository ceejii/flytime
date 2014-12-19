using System;

namespace FlyableHours.Helpers
{
    public class WeatherHelper
    {

        public static int? CalculateWindChill(int temperature, float windSpeed)
        {
            if (temperature > 10)
            {
                return null;
            }
            if (windSpeed < 4.8f)
            {
                return null;
            }
            int result = Math.Round(13.126665f + 0.6215f * temperature - 13.924748f * Math.Pow(windSpeed, 0.16f) + 0.4875195f * temperature * Math.Pow(windSpeed, 0.16f), 0);
            return result;
        }
    }
}
