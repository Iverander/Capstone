using System;
using System.Collections;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    //[DisallowMultipleComponent]
    public abstract class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] public Vector2 speed { get; protected set; } = new Vector2(4, 7);
        [field: SerializeField, ReadOnly] public Vector3 moveDirection { get; private set; }
        public abstract Vector3 ConvertedDirection { get; }
        protected bool sprinting => Player.state.HasFlag(State.Sprinting);
        
        protected float currentSpeed => sprinting ? speed.y : speed.x;
        
        [Header("Jumping")]
        [SerializeField] protected float jumpForce = 4;
        
        [Header("Turning")]
        [SerializeField] protected float rotationSpeed = 7;
        [SerializeField, ReadOnly] protected float rotationVelocity;
        
        protected Rigidbody rb => Player.instance.rb;
        protected Camera cam => Player.instance.cam;
        protected Animator animator => Player.instance.animator;
        
        protected virtual void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Player.input.onMove.AddListener(UpdateMovement);
            Player.AddState(State.Sprinting);
            Player.input.onSprint.AddListener(ToggleSprint);
            Player.input.onJump.AddListener(StartJump);
        }

        void StartJump()
        {            
            if (Player.state.HasFlag(State.Falling) || !Player.state.HasFlag(State.Grounded)) return;
            StartCoroutine(Jump());
        }

        IEnumerator Jump()
        {
            animator.SetTrigger("Jump");
            Player.AddState(State.Jumping);
            rb.AddForce(jumpForce * rb.mass * Vector3.up, ForceMode.Impulse);
            
            yield return new WaitForFixedUpdate();
            
            while (rb.linearVelocity.y >= .5)
            {
                yield return new WaitForFixedUpdate();
            }
            
            Player.RemoveState(State.Jumping);
        }

         void ToggleSprint()
        {
            if (!sprinting)
            {
                Player.AddState(State.Sprinting);
            }
            else
            {
                Player.RemoveState(State.Sprinting);
            }
        }
        void UpdateMovement(Vector2 value)
        {
            moveDirection = new Vector3(value.x, 0, value.y).normalized;
            
            if(moveDirection.magnitude > .1f)
                Player.AddState(State.Walking);
            else
                Player.RemoveState(State.Walking);
        }

        void FixedUpdate()
        {
            animator.SetFloat("Speed", rb.linearVelocity.magnitude / speed.y);
            animator.SetFloat("DirectionX", moveDirection.x);
            animator.SetFloat("DirectionY", moveDirection.z);
            
            if(Player.instance.stunned) return;
            Movement();
            //Player.instance.dash.direction = ConvertedDirection;
        }

        private void Update()
        {
            if(Player.instance.stunned) return;
            LimitSpeed();
        }

        protected abstract void Movement();
        
        
        void LimitSpeed()
        {
            Vector3 maxVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (maxVelocity.magnitude > currentSpeed)
            {
                Vector3 newSpeed = maxVelocity.normalized * currentSpeed;
                rb.linearVelocity = new Vector3(newSpeed.x, rb.linearVelocity.y, newSpeed.z);
            }
        }
        
        protected void FaceDirection(Vector3 target, bool basedOnMovement)
        {
            if(Mathf.Abs(transform.eulerAngles.y - target.y) > 10f)
            {
                Player.AddState(State.Turning);
                
                if(basedOnMovement && moveDirection == Vector3.zero)
                {
                    rb.angularVelocity = Vector3.zero;
                    Player.RemoveState(State.Turning);
                    return;
                }
                    
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.y, ref rotationVelocity, 1 / rotationSpeed);
                transform.eulerAngles = new Vector3(0, rotation, 0);
            }
            else
            {
                Player.RemoveState(State.Turning);
            }
        }
    }
}
