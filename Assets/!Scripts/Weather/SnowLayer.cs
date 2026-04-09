using System;
using UnityEngine;

namespace Capstone
{
    public class SnowLayer : MonoBehaviour
    {
        private Material snowMat;
        private void Start()
        {
            snowMat = GetComponent<Renderer>().materials[1];
            
            if (Settings.mapSettings.weatherType == WeatherType.Snowing)
            {
                snowMat.SetFloat("_SnowAmount", 1);
            }
            else
            {
                snowMat.SetFloat("_SnowAmount",.01f); 
            }
        }
    }
}
