using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class IsometricMovement : PlayerMovement
    {
        public override Vector3 ConvertedDirection => Quaternion.AngleAxis(45, Vector3.up) * moveDirection;
        private Vector3 worldMousePosition;
        Vector2 screenMousePosition;
        
        [SerializeField] LayerMask groundLayer = 1<<6;//bitshift '1' 6 times to the left: 00000001 => 01000000

        private GameObject indicator;
        protected override void Start()
        {
            base.Start();
            Cursor.lockState = CursorLockMode.Confined;
            Player.input.onMousePosition.AddListener(GetMousePosition);
            
            indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indicator.SetActive(false);
        }

        private void Update()
        {
            Ray ray = Player.instance.cam.ScreenPointToRay(screenMousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                worldMousePosition = hit.point;
            }
            indicator.transform.position = worldMousePosition;
        }

        private void GetMousePosition(Vector2 arg0)
        {
            screenMousePosition = arg0;
        }

        protected override void Movement()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            transform.LookAt(worldMousePosition);
            //FaceDirection(Quaternion.LookRotation(worldMousePosition, transform.up).eulerAngles, false);
        }
        
    }
}
