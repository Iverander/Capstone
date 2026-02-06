using System;
using System.Collections;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] Vector2 speed = new Vector2(5, 10);
        [field: SerializeField, ReadOnly] public Vector3 moveDirection { get; private set; }
        [SerializeField, ReadOnly] private bool sprinting;
        
        Vector3 ConvertedDirection => transform.forward * moveDirection.z + transform.right * moveDirection.x;
        float currentSpeed => sprinting ? speed.y : speed.x;
        
        [Header("Jumping")]
        [SerializeField] float jumpForce;
        [SerializeField, ReadOnly] bool jumping;
        
        [Header("Turning")]
        [SerializeField] float rotationSpeed;
        [SerializeField, ReadOnly] float rotationVelocity;
        [SerializeField, ReadOnly] bool turning;
        
        Rigidbody rb => Player.instance.rb;
        Camera cam => Player.instance.cam;
        
        private void Start()
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
            jumping = true;
            rb.AddForce(jumpForce * rb.mass * Vector3.up, ForceMode.Impulse);
            
            yield return new WaitForFixedUpdate();
            
            while (rb.linearVelocity.y != 0)
            {
                yield return new WaitForFixedUpdate();
            }
            
            jumping = false;
        }

        private void ToggleSprint()
        {
            sprinting = !sprinting;
        }
        void UpdateMovement(Vector2 value)
        {
            moveDirection = new Vector3(value.x, 0, value.y).normalized;
        }

        private void FixedUpdate()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            LimitSpeed();
            
            if(!turning)
            {
                StartCoroutine(FaceCameraDirection(true));
            }
        }
        
        
        void LimitSpeed()
        {
            Vector3 maxVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (maxVelocity.magnitude > currentSpeed)
            {
                Vector3 newSpeed = maxVelocity.normalized * currentSpeed;
                rb.linearVelocity = new Vector3(newSpeed.x, rb.linearVelocity.y, newSpeed.z);
            }
        }

        public IEnumerator FaceCameraDirection(bool basedOnMovement)
        {
            turning = true;

            while(Mathf.Abs(transform.eulerAngles.y - cam.transform.eulerAngles.y) > 10f)
            {
                if(basedOnMovement)
                {
                    if (moveDirection == Vector3.zero)
                    {
                        rb.angularVelocity = Vector3.zero;
                        break;
                    }
                }
                    
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.transform.eulerAngles.y, ref rotationVelocity, 1 / rotationSpeed);
                transform.eulerAngles = new Vector3(0, rotation, 0);

                yield return null;
            }

            turning = false;
        }
    }
}
