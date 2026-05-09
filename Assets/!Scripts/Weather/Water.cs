using UnityEngine;

namespace Capstone
{
    public class Water : MonoBehaviour
    {
        [SerializeField] private float defaultRippleStrength;
        private Renderer waterRenderer;

        private void Start()
        {
            waterRenderer = GetComponent<Renderer>();
            SetRippleStrength(defaultRippleStrength);

            if (Settings.active.mapSettings.weatherType == WeatherType.Raining)
                SetRippleStrength(1);
        }

        public void SetRippleStrength(float value)
        {
            waterRenderer.material.SetFloat("_RippleStrength", value);
        }
    }
}