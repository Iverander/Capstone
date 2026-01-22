using System;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed = 10;
        
        [field: SerializeField, ReadOnly] public Vector2 moveDirection { get; private set; }
        
        private void Start()
        {
            Player.input.onMove.AddListener(UpdateMovement);
        }

        private void FixedUpdate()
        {
            Player.instance.rb.AddForce(100 * speed * Time.fixedDeltaTime * moveDirection, ForceMode.Force);
        }

        void UpdateMovement(Vector2 value)
        {
            moveDirection = value;
        }
    }
}
