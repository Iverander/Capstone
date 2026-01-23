using System;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private bool sprint;
        [SerializeField] Vector2 speed = new Vector2(5, 10);
        
        [field: SerializeField, ReadOnly] public Vector3 moveDirection { get; private set; }
        Vector3 ConvertedDirection => transform.forward * moveDirection.z + transform.right * moveDirection.x;
        
        float currentSpeed => sprint ? speed.y : speed.x;
        
        Rigidbody rb => Player.instance.rb;
        
        private void Start()
        {
            Player.input.onMove.AddListener(UpdateMovement);
            Player.input.onSprint.AddListener(ToggleSpring);
        }

        private void ToggleSpring()
        {
            sprint = !sprint;
        }

        private void FixedUpdate()
        {
            Player.instance.rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            
            
            LimitSpeed();
        }
        

        void UpdateMovement(Vector2 value)
        {
            moveDirection = new Vector3(value.x, 0, value.y).normalized;
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
    }
}
