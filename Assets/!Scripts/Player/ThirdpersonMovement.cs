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
            
            if(!turning)
            {
                StartCoroutine(FaceCameraDirection(true));
            }
        }
        

        protected IEnumerator FaceCameraDirection(bool basedOnMovement)
        {
            Player.AddState(State.Turning);

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

            Player.RemoveState(State.Turning);
        }
    }
}
