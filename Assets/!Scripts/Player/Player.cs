using System;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    [Flags]
    public enum State
    {
        None = 0 << 0,
        Sprinting = 1 << 0,
        Jumping = 1 << 1,
        Turning = 1 << 2,
        Falling = 1 << 3,
    }
    [DefaultExecutionOrder(-10000)]
    public class Player : MonoBehaviour
    {
        
        public static Player instance;
        public static InputReader input => instance.inputReader;
        public InputReader inputReader = new();
        
        public static CameraType cameraType => instance.cameraSettings.cameraType;

        [ReadOnly]public State playerState;
        public static State state
        {
            get { return instance.playerState;}
            set { instance.playerState = value; }
        }

        public Camera cam => cameraSettings.activeCamera;
        public Rigidbody rb { get; private set; }
        public CameraSettings cameraSettings { get; private set; }
        
        
        void Start()
        {
            instance = this;
            inputReader.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
            
            rb = GetComponent<Rigidbody>();
            cameraSettings = GetComponent<CameraSettings>();

            switch (cameraType)
            {
                case CameraType.ThirdPerson:
                    gameObject.AddComponent<ThirdpersonMovement>();
                    break;
                case CameraType.Isometric:
                    gameObject.AddComponent<IsometricMovement>();
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (rb.linearVelocity.y < 0 && !state.HasFlag(State.Falling))
            {
                AddState(State.Falling);
            }
            else
            {
                RemoveState(State.Falling);
            }
        }

        private void OnDestroy()
        {
            inputReader.Disable();
        }

        public static void AddState(State state)
        {
            Player.state |= state;
        }
        public static void RemoveState(State state)
        {
            Player.state &= ~state;
        }
    }
}
