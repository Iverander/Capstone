using System;
using System.Collections;
using System.Threading.Tasks;
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

        private GameObject current;
        
        [HideInInspector] public Camera activeCamera; 
        
        [Header("ThirdPerson Settings")]
        [SerializeField, ShowIf(nameof(cameraType), CameraType.ThirdPerson)] GameObject thirdPersonCamera;
        [Header("Isometric Settings")]
        [SerializeField, ShowIf(nameof(cameraType), CameraType.Isometric)] GameObject isometricCamera;
        
        private void Start()
        {
            ChangeCamera(cameraType);
        }
/*
        private async void OnValidate()
        {
            await Task.Delay(10);
            ChangeCamera(cameraType);
        }*/

        public void ChangeCamera(CameraType type)
        {
            cameraType = type;

            if (current != null)
            {
                DestroyImmediate(current);
            }
            
            switch (cameraType)
            {
                case CameraType.ThirdPerson:
                    current = Instantiate(thirdPersonCamera, transform);
                    activeCamera = current.GetComponentInChildren<Camera>();
                    break;
                case CameraType.Isometric:
                    current = Instantiate(isometricCamera, transform);
                    activeCamera = current.GetComponentInChildren<Camera>();
                    break;
            }
        }
    }
}
