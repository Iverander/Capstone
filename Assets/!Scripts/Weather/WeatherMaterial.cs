using System;
using UnityEngine;

namespace Capstone
{
    public class WeatherMaterial : MonoBehaviour
    {
        [SerializeField] private EvolvingMaterial[] materials;
        private void Start()
        {
            foreach (EvolvingMaterial material in materials)
            {
                if (Settings.active.mapSettings.weatherType != material.weatherCondition)
                {
                    material.material.SetFloat("_"+material.variableName, material.minMax.x);
                }
                else
                {
                    material.material.SetFloat("_" + material.variableName, material.minMax.y);
                }
            }
            

        }

        [Serializable]
        public struct EvolvingMaterial
        {
            public Material material;
            public string variableName;
            public Vector2 minMax;
            public WeatherType weatherCondition;
        }
    }
}
