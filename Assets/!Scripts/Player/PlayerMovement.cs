using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    //[DisallowMultipleComponent]
    public abstract class PlayerMovement : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public Vector3 moveDirection { get; private set; }

        [Header("Jumping")] [SerializeField] protected float jumpForce = 6;

        [Header("Turning")] [SerializeField] protected float rotationSpeed = 7;

        [SerializeField] [ReadOnly] protected float rotationVelocity;
        [Header("Movement")] public Vector2 speed { get; protected set; } = new(4, 7);
        public abstract Vector3 ConvertedDirection { get; }
        protected bool sprinting => Player.state.HasFlag(State.Sprinting);

        protected float currentSpeed => sprinting ? speed.y : speed.x;

        protected Rigidbody rb => Player.instance.rb;
        protected Camera cam => Player.instance.cam;
        protected Animator animator => Player.instance.animator;

        private bool WalkCondition =>
            !(Player.instance.stunned || !Player.instance.playerState.HasFlag(State.Grounded));

        protected virtual void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Player.input.onMove.AddListener(UpdateMovement);
            Player.AddState(State.Sprinting);
            Player.input.onSprint.AddListener(ToggleSprint);
            Player.input.onJump.AddListener(StartJump);
        }

        private void Update()
        {
            if (!WalkCondition) return;
            LimitSpeed();
        }

        private void FixedUpdate()
        {
            animator.SetFloat("Speed", rb.linearVelocity.magnitude / speed.y);
            animator.SetFloat("DirectionX", moveDirection.x);
            animator.SetFloat("DirectionY", moveDirection.z);
            //Debug.Log(WalkCondition);

            if (!WalkCondition) return;
            Movement();
            //Player.instance.dash.direction = ConvertedDirection;
        }

        private void StartJump()
        {
            if (Player.state.HasFlag(State.Falling) || !Player.state.HasFlag(State.Grounded)) return;
            StartCoroutine(Jump());
        }

        private IEnumerator Jump()
        {
            animator.SetTrigger("Jump");
            Player.AddState(State.Jumping);
            rb.AddForce(jumpForce * rb.mass * Vector3.up, ForceMode.Impulse);

            yield return new WaitForFixedUpdate();

            while (rb.linearVelocity.y >= .5) yield return new WaitForFixedUpdate();

            Player.RemoveState(State.Jumping);
        }

        private void ToggleSprint()
        {
            if (!sprinting)
                Player.AddState(State.Sprinting);
            else
                Player.RemoveState(State.Sprinting);
        }

        private void UpdateMovement(Vector2 value)
        {
            moveDirection = new Vector3(value.x, 0, value.y).normalized;


            //rb.linearVelocity = ConvertedDirection * currentSpeed;

            if (moveDirection.magnitude > .1f)
                Player.AddState(State.Walking);
            else
                Player.RemoveState(State.Walking);
        }

        protected abstract void Movement();


        private void LimitSpeed()
        {
            var maxVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (maxVelocity.magnitude > currentSpeed)
            {
                var newSpeed = maxVelocity.normalized * currentSpeed;
                rb.linearVelocity = new Vector3(newSpeed.x, rb.linearVelocity.y, newSpeed.z);
            }
        }

        protected void FaceDirection(Vector3 target, bool basedOnMovement)
        {
            if (Mathf.Abs(transform.eulerAngles.y - target.y) > 10f)
            {
                Player.AddState(State.Turning);

                if (basedOnMovement && moveDirection == Vector3.zero)
                {
                    rb.angularVelocity = Vector3.zero;
                    Player.RemoveState(State.Turning);
                    return;
                }

                var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.y, ref rotationVelocity,
                    1 / rotationSpeed);
                transform.eulerAngles = new Vector3(0, rotation, 0);
            }
            else
            {
                Player.RemoveState(State.Turning);
            }
        }
    }
}