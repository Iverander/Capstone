using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capstone
{
    public enum WeatherType
    {
        Sunny,
        Raining,
    }
    
    public class WeatherManager : MonoBehaviour
    {
        [SerializedDictionary, SerializeField] SerializedDictionary<WeatherType, Weather> weatherPrefabs;

        void Start()
        {
            Instantiate(weatherPrefabs[Settings.mapSettings.weatherType].gameObject, Vector3.zero, Quaternion.identity);
        }
    }
}
