using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capstone
{
    public enum WeatherType
    {
        Sunny,
        Raining,
        Snowing
    }
    
    public class WeatherManager : MonoBehaviour
    {
        [SerializedDictionary, SerializeField] SerializedDictionary<WeatherType, Weather> weatherPrefabs;

        void Start()
        {
            Instantiate(weatherPrefabs[Settings.active.mapSettings.weatherType].gameObject, Vector3.zero, Quaternion.identity);
        }
    }
}
