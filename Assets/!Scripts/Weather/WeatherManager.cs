using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Capstone
{
    public class WeatherManager : MonoBehaviour
    {
        [SerializeField] Weather weatherOverride;
        [SerializedDictionary, SerializeField] SerializedDictionary<Weather, GameObject> weatherPrefabs;

        void Start()
        {
            if (LevelSettings.CurrentWeather == Weather.None)
            {
                LevelSettings.ChangeCurrentWeather(weatherOverride);
            }
            Instantiate(weatherPrefabs[LevelSettings.CurrentWeather], Vector3.zero, Quaternion.identity);
        }
    }
}
