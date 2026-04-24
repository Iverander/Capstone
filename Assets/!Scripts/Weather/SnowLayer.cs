using System;
using UnityEngine;

namespace Capstone
{
    public class SnowLayer : MonoBehaviour
    {
        private Material snowMat;
        private void Start()
        {
            if (Settings.active.mapSettings.weatherType != WeatherType.Snowing) return;
            
            snowMat = GetComponent<Renderer>().materials[1];
            snowMat.SetFloat("_SnowAmount", 1);
        }
    }
}
