using System;
using UnityEngine;

namespace Capstone
{
    public class Water : MonoBehaviour
    {
        Renderer waterRenderer;
        private static Action<float> RippleUpdated;
        
        void Start()
        {
            waterRenderer = GetComponent<Renderer>();

            RippleUpdated += (ripple) =>
            {
                waterRenderer.material.SetFloat("_RippleStrength", ripple);
            };
        }

        public static void SetRippleStrength(float value)
        {
            RippleUpdated?.Invoke(value);
        }
    }
}
