using System;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class Weather : MonoBehaviour
    {
        [SerializeField] protected Material skybox;

        void Start()
        {
            Apply();
        }
        
        [Button]
        public virtual void Apply()
        {
            if (skybox)
                RenderSettings.skybox = skybox;
        }
    }
}
