using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public void Randomize()
        {
            map =  (Map)Random.Range(1, Enum.GetValues(typeof(Map)).Length);
            weatherType = (WeatherType)Random.Range(0, Enum.GetValues(typeof(WeatherType)).Length);
            obstacles = Random.Range(0, 2) == 1;
        }
    }
    [Serializable]
    public class Settings
    {
        public static Settings active = new(true);
        public ShaderType shaderType; 
        [field: SerializeField] public MapSettings mapSettings { get; private set; } = new();

        public Settings(bool randomize)
        {
            if(randomize)
                Randomize();
        }

        public void Randomize()
        {
            shaderType = (ShaderType)Random.Range(0, Enum.GetValues(typeof(ShaderType)).Length);
            mapSettings.Randomize();
        }
    }
}
