using System;
using NaughtyAttributes;
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
        
        [Header("Thirdperson Settings")]
        [SerializeField, ShowIf(nameof(cameraType), CameraType.ThirdPerson)] GameObject thirdPersonCamera;
        [Header("Isometric Settings")]
        [SerializeField, ShowIf(nameof(cameraType), CameraType.Isometric)] GameObject isometricCamera;
        
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
