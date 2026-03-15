using System;
using UnityEngine;

namespace Capstone
{
    public class Weather : MonoBehaviour
    {
        [SerializeField] protected Material skybox;

        protected virtual void Start()
        {
            if (skybox)
                RenderSettings.skybox = skybox;
        }
    }
}
