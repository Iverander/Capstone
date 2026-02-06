using System;
using UnityEngine;

namespace Capstone
{
    [DefaultExecutionOrder(-10000)]
    public class Player : MonoBehaviour
    {
        public static Player instance;
        public static InputReader input => instance.inputReader;
        public InputReader inputReader = new();

        public Camera cam { get; private set; }
        public Rigidbody rb { get; private set; }
        
        
        void Start()
        {
            instance = this;
            inputReader.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
            
            rb = GetComponent<Rigidbody>();
            cam = GetComponentInChildren<Camera>();
        }

        private void OnDestroy()
        {
            inputReader.Disable();
        }
    }
}
