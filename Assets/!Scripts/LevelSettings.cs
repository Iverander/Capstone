using UnityEngine;

namespace Capstone
{
    public enum Weather
    {
        None,
        Raining,
    }
    
    public static class LevelSettings
    {
        public static Weather CurrentWeather { get; private set; } = Weather.None;

        public static void ChangeCurrentWeather(Weather weather)
        {
            Debug.Log($"Changing weather to {weather}");
            CurrentWeather = weather;
        }
    }
}
