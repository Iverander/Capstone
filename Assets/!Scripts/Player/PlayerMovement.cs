using System;
using System.Collections;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    [DisallowMultipleComponent]
    public abstract class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] protected Vector2 speed = new Vector2(5, 10);
        [field: SerializeField, ReadOnly] public Vector3 moveDirection { get; private set; }
        protected bool sprinting => Player.state.HasFlag(State.Sprinting);
        
        protected Vector3 ConvertedDirection => transform.forward * moveDirection.z + transform.right * moveDirection.x;
        protected float currentSpeed => sprinting ? speed.y : speed.x;
        
        [Header("Jumping")]
        [SerializeField] protected float jumpForce = 7;
        
        [Header("Turning")]
        [SerializeField] protected float rotationSpeed = 7;
        [SerializeField, ReadOnly] protected float rotationVelocity;
        protected bool turning => Player.state.HasFlag(State.Turning);
        
        protected Rigidbody rb => Player.instance.rb;
        protected Camera cam => Player.instance.cam;
        
        protected virtual void Start()
        {
            Player.input.onMove.AddListener(UpdateMovement);
            Player.input.onSprint.AddListener(ToggleSprint);
            Player.input.onJump.AddListener(StartJump);
        }

        private void StartJump()
        {
            StartCoroutine(Jump());
        }

        private IEnumerator Jump()
        {
            Player.AddState(State.Jumping);
            rb.AddForce(jumpForce * rb.mass * Vector3.up, ForceMode.Impulse);
            
            yield return new WaitForFixedUpdate();
            
            while (rb.linearVelocity.y != 0)
            {
                yield return new WaitForFixedUpdate();
            }
            
            Player.RemoveState(State.Jumping);
        }

        private void ToggleSprint()
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
        }

        private void FixedUpdate()
        {
            Movement();
        }

        protected abstract void Movement();
        
        
        protected void LimitSpeed()
        {
            Vector3 maxVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (maxVelocity.magnitude > currentSpeed)
            {
                Vector3 newSpeed = maxVelocity.normalized * currentSpeed;
                rb.linearVelocity = new Vector3(newSpeed.x, rb.linearVelocity.y, newSpeed.z);
            }
        }
    }
}
