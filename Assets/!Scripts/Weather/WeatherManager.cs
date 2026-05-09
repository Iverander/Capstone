using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Capstone
{
    public enum WeatherType
    {
        Sunny,

        Raining
        //Snowing
    }

    public class WeatherManager : MonoBehaviour
    {
        [SerializedDictionary] [SerializeField]
        private SerializedDictionary<WeatherType, Weather> weatherPrefabs;

        private void Start()
        {
            Instantiate(weatherPrefabs[Settings.active.mapSettings.weatherType].gameObject, Vector3.zero,
                Quaternion.identity);
        }
    }
}