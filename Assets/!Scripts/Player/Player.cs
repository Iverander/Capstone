using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    [Flags]
    public enum State
    {
        None = 0,
        Sprinting = 1 << 0,
        Grounded = 1 << 1,
        Turning = 1 << 2,
        Falling = 1 << 3,
        Jumping = 1 << 4,
        Walking = 1 << 5
    }

    [DefaultExecutionOrder(-10000)]
    public class Player : Creature
    {
        public static Player instance;
        public InputReader inputReader = new();

        [ReadOnly] public State playerState;

        public float afkTime;
        public static InputReader input => instance.inputReader;

        public static CameraType cameraType => instance.cameraSettings.cameraType;

        public static State state
        {
            get => instance.playerState;
            set => instance.playerState = value;
        }

        public Camera cam => cameraSettings.activeCamera;
        public CameraSettings cameraSettings { get; private set; }
        public PlayerMovement movement { get; private set; }
        public PlayerCombat combat { get; private set; }
        public PlayerModifier modifier { get; private set; }

        private void Start()
        {
            instance = this;
            inputReader.Enable();

            cameraSettings = GetComponent<CameraSettings>();
            combat = GetComponent<PlayerCombat>();
            modifier = GetComponent<PlayerModifier>();
            cameraSettings.CameraChanged += CameraChanged;
        }

        private void FixedUpdate()
        {
            Ray groundRay = new(transform.position + -Vector3.down * .1f, Vector3.down);
            Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red);
            if (Physics.Raycast(groundRay, .2f))
                AddState(State.Grounded);
            else
                RemoveState(State.Grounded);
            if (!state.HasFlag(State.Falling))
            {
                if (rb.linearVelocity.y <= -.5)
                    AddState(State.Falling);
            }
            else
            {
                if (rb.linearVelocity.y > -.5)
                    RemoveState(State.Falling);
            }

            if ((state & ~State.Sprinting & ~State.Grounded) == State.None) afkTime += Time.fixedDeltaTime;
        }

        private void OnDestroy()
        {
            inputReader.Disable();
        }

        private void CameraChanged()
        {
            if (movement != null) Destroy(movement);

            switch (cameraType)
            {
                case CameraType.ThirdPerson:
                    movement = gameObject.AddComponent<ThirdpersonMovement>();
                    break;
                case CameraType.Isometric:
                    movement = gameObject.AddComponent<IsometricMovement>();
                    break;
            }

            playerState = 0;
        }

        public static void AddState(State state)
        {
            Player.state |= state;
        }

        public static void RemoveState(State state)
        {
            Player.state &= ~state;
        }

        public override IEnumerator Stun(float durationSeconds)
        {
            stunned = true;
            yield return new WaitForSeconds(durationSeconds);
            stunned = false;
            //rb.AddForce((transform.position - origin) * (knockback * 10), ForceMode.Force);
        }
    }
}