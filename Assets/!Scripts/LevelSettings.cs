using UnityEngine;

namespace Capstone
{
    public enum WeatherType
    {
        None,
        Raining,
    }
    
    public static class LevelSettings
    {
        public static WeatherType CurrentWeatherType { get; private set; } = WeatherType.None;

        public static void ChangeCurrentWeather(WeatherType weatherType)
        {
            Debug.Log($"Changing weather to {weatherType}");
            CurrentWeatherType = weatherType;
        }
    }
}
