using System.Collections;
using UnityEngine;

namespace Capstone
{
    public class ThirdpersonMovement : PlayerMovement
    {
        protected override Vector3 ConvertedDirection => transform.forward * moveDirection.z + transform.right * moveDirection.x;
        
        protected override void Movement()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);

            FaceDirection(cam.transform.eulerAngles, true);
        }
    }
}
