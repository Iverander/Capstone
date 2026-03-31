using System;
using UnityEngine;

namespace Capstone
{
    public enum ShaderType
    {
        HLSL,
        ShaderGraph
    }
    public enum Map
    {
        Mountain
    }

    [Serializable]
    public class MapSettings
    {
        public WeatherType weatherType = WeatherType.Sunny;
        public bool obstacles = true;

        public override string ToString()
        {
            string result = "";
            foreach (var field in typeof(MapSettings).GetFields())
            {
                result += "["+field.Name + ": " + field.GetValue(this) + "] \n";
            }
            
            return result;
        }
    }
    public static class LevelSettings
    {
        public static ShaderType shaderType;
        public static MapSettings CurrentMapSettings { get; private set; } = new();
        

        public static void ChangeCurrentWeather(WeatherType weatherType)
        {
            Debug.Log($"Changing weather to {weatherType}");
            CurrentMapSettings.weatherType = weatherType;
        }
        public static void ToggleObstacles(bool toggle)
        {
            CurrentMapSettings.obstacles = toggle;
        }
    }
}
