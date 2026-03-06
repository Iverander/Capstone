using System;
using UnityEngine;

namespace Capstone
{
    public class Water : MonoBehaviour
    {
        Renderer waterRenderer;
        private static Action<float> RippleUpdated;
        
        [SerializeField] float defaultRippleStrength = 0;
        
        void Start()
        {
            waterRenderer = GetComponent<Renderer>();
            waterRenderer.material.SetFloat("_RippleStrength", defaultRippleStrength);

            RippleUpdated += UpdateRipple;
        }

        private void OnDestroy()
        {
            RippleUpdated -= UpdateRipple;
        }

        private void UpdateRipple(float ripple)
        {
            waterRenderer.material.SetFloat("_RippleStrength", ripple);
        }

        public static void SetRippleStrength(float value)
        {
            RippleUpdated?.Invoke(value);
        }
    }
}
