using System.Collections;
using UnityEngine;

namespace Capstone
{
    public class ThirdpersonMovement : PlayerMovement
    {
        [SerializeField] private bool lockedToCamera = true;

        Vector3 cameraForward => new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 cameraRight => new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;

        public override Vector3 ConvertedDirection => cameraForward * moveDirection.z + cameraRight * moveDirection.x;
        
        protected override void Start()
        {
            base.Start();
            Player.input.onCameraLock.AddListener(ToggleCameraLock);
        }

        protected override void Movement()
        {
            //Debug.Log(cam.transform.forward);

            //rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            rb.linearVelocity = ConvertedDirection * Time.fixedDeltaTime * currentSpeed * 45;
            
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
