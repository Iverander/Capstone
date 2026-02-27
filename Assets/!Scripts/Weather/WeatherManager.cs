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
            if (LevelSettings.CurrentMapSettings.weatherType == WeatherType.None)
            {
                LevelSettings.ChangeCurrentWeather(weatherTypeOverride);
            }
            Instantiate(weatherPrefabs[LevelSettings.CurrentMapSettings.weatherType].gameObject, Vector3.zero, Quaternion.identity);
        }
    }
}
