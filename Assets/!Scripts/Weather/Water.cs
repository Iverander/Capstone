using System;
using UnityEngine;

namespace Capstone
{
    public class Water : MonoBehaviour
    {
        Renderer waterRenderer;
               
        [SerializeField] float defaultRippleStrength = 0;
        
        void Start()
        {
            waterRenderer = GetComponent<Renderer>();
            SetRippleStrength(defaultRippleStrength);
            
            if(Settings.mapSettings.weatherType == WeatherType.Raining)
                SetRippleStrength(1);
        }
        public void SetRippleStrength(float value)
        {
            waterRenderer.material.SetFloat("_RippleStrength", value);
        }
    }
}
