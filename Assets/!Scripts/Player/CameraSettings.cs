using System;
using UnityEngine;

namespace Capstone
{
    public enum CameraType
    {
        ThirdPerson,
        Isometric,
    }
    public class CameraSettings : MonoBehaviour
    {
        
        public CameraType cameraType;
        
        [HideInInspector] public Camera activeCamera; 
        
        [Header("ThirdPerson")]
        [SerializeField] GameObject thirdPersonCamera;
        [Header("Isometric")]
        [SerializeField] GameObject isometricCamera;
        
        private void Start()
        {
            switch (cameraType)
            {
                case CameraType.ThirdPerson:
                    activeCamera = Instantiate(thirdPersonCamera, transform).GetComponentInChildren<Camera>();
                    break;
                case CameraType.Isometric:
                    activeCamera = Instantiate(isometricCamera, transform).GetComponentInChildren<Camera>();
                    break;
            }
        }
    }
}
