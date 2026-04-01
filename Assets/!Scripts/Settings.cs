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
        Showcase,
        Mountain
    }

    [Serializable]
    public class MapSettings
    {
        public Map map = Map.Mountain;
        public WeatherType weatherType = WeatherType.Sunny;
        public bool obstacles = true;

        public void SetWeather(WeatherType weatherType)
        {
            Debug.Log($"Changing weather to {weatherType}");
            this.weatherType = weatherType;
        }
        public void ToggleObstacles(bool toggle)
        {
            this.obstacles = toggle;
        }

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
    public static class Settings
    {
        public static ShaderType shaderType; 
        public static MapSettings mapSettings { get; private set; } = new();

        public static string ToString()
        {
            return 
            $"[Shader type: {shaderType}]" +
            $"[MapSettings: {mapSettings.ToString()}]";
        }
    }
}
