using System.Collections;
using UnityEngine;

namespace Capstone
{
    public class ThirdpersonMovement : PlayerMovement
    {
        [SerializeField] private bool lockedToCamera = true;
        protected override Vector3 ConvertedDirection => transform.forward * moveDirection.z + transform.right * moveDirection.x;
        
        protected override void Start()
        {
            base.Start();
            Player.input.onCameraLock.AddListener(ToggleCameraLock);
        }

        protected override void Movement()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            if(lockedToCamera)
                transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
            //else
                //FaceDirection(cam.transform.eulerAngles, true);
        }

        void ToggleCameraLock()
        {
            lockedToCamera = !lockedToCamera;
        }
    }
}
