using System;
using UnityEngine;

namespace Capstone
{
    public enum WeatherType
    {
        None,
        Raining,
    }
    
    [Serializable]
    public class MapSettings
    {
        public WeatherType weatherType = WeatherType.None;

        public override string ToString()
        {
            string returnString = "";
            
            returnString += $"Weather: {weatherType.ToString()}\n";
            
            return returnString;
        }
    }
    public static class LevelSettings
    {
        public static MapSettings CurrentMapSettings { get; private set; } = new();
        

        public static void ChangeCurrentWeather(WeatherType weatherType)
        {
            Debug.Log($"Changing weather to {weatherType}");
            CurrentMapSettings.weatherType = weatherType;
        }
    }
}
