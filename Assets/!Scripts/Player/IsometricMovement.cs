using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class IsometricMovement : PlayerMovement
    {
        protected override Vector3 ConvertedDirection => Quaternion.AngleAxis(45, Vector3.up) * moveDirection;
        private Vector3 worldMousePosition;
        
        [SerializeField] LayerMask groundLayer = 1<<6;//bitshift '1' 6 times to the left: 00000001 => 01000000
        

        protected override void Start()
        {
            base.Start();
            Cursor.lockState = CursorLockMode.Confined;
            Player.input.onMousePosition.AddListener(GetMousePosition);
        }

        private void GetMousePosition(Vector2 arg0)
        {
            Ray ray = Player.instance.cam.ScreenPointToRay(arg0);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                worldMousePosition = hit.point;
            }
        }

        protected override void Movement()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            FaceDirection(Quaternion.LookRotation(worldMousePosition, transform.up).eulerAngles, false);
        }
        
    }
}
