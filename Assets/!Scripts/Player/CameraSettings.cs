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
            thirdPersonCamera.SetActive(false);
            isometricCamera.SetActive(false);

            
            switch (cameraType)
            {
                case CameraType.ThirdPerson:
                    thirdPersonCamera.SetActive(true);
                    activeCamera = thirdPersonCamera.GetComponentInChildren<Camera>();
                    break;
                case CameraType.Isometric:
                    isometricCamera.SetActive(true);
                    activeCamera = isometricCamera.GetComponentInChildren<Camera>();
                    break;
            }
        }
    }
}
