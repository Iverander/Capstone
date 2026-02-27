using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capstone
{
    public class WeatherManager : MonoBehaviour
    {
        [FormerlySerializedAs("weatherOverride")] [SerializeField] WeatherType weatherTypeOverride;
        [SerializedDictionary, SerializeField] SerializedDictionary<WeatherType, Weather> weatherPrefabs;

        void Start()
        {
            if (LevelSettings.CurrentWeatherType == WeatherType.None)
            {
                LevelSettings.ChangeCurrentWeather(weatherTypeOverride);
            }
            Instantiate(weatherPrefabs[LevelSettings.CurrentWeatherType].gameObject, Vector3.zero, Quaternion.identity);
        }
    }
}
