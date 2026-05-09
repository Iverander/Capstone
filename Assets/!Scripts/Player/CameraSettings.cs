using System;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public enum CameraType
    {
        ThirdPerson,
        Isometric
    }

    public class CameraSettings : MonoBehaviour
    {
        public CameraType cameraType;

        [HideInInspector] public Camera activeCamera;

        [Header("ThirdPerson Settings")] [SerializeField] [ShowIf(nameof(cameraType), CameraType.ThirdPerson)]
        private GameObject thirdPersonCamera;

        [Header("Isometric Settings")] [SerializeField] [ShowIf(nameof(cameraType), CameraType.Isometric)]
        private GameObject isometricCamera;

        public Action CameraChanged;

        private GameObject current;

        private void Start()
        {
            ChangeCamera(cameraType);

            Player.input.onCameraChange.AddListener(SwapCamera);
        }

        private void SwapCamera()
        {
            var newCameraType = cameraType + 1;

            if ((int)newCameraType > Enum.GetValues(typeof(CameraType)).Length - 1) newCameraType = 0;

            ChangeCamera(newCameraType);
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

            if (current != null) DestroyImmediate(current);

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

            CameraChanged?.Invoke();
        }
    }
}