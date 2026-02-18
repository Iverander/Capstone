using System.Collections;
using UnityEngine;

namespace Capstone
{
    public class IsometricMovement : PlayerMovement
    {
        protected Vector3 ConvertedDirection => Quaternion.AngleAxis(45, Vector3.up) * moveDirection;

        protected override void Movement()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            LimitSpeed();
            
            if(!turning)
            {
                StartCoroutine(FaceWalkDirection(true));
            }
        }
        
        IEnumerator FaceWalkDirection(bool basedOnMovement)
        {
            Player.AddState(State.Turning);
            
            float target = GetTargetRotation();
            
            while(Mathf.Abs(transform.eulerAngles.y - target) > 10f)
            {
                if(basedOnMovement)
                {
                    if (moveDirection == Vector3.zero)
                    {
                        rb.angularVelocity = Vector3.zero;
                        break;
                    }
                }
                
                target = GetTargetRotation();
                
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref rotationVelocity, 1 / rotationSpeed);
                transform.eulerAngles = new Vector3(0, rotation, 0);

                yield return null;
            }
            
            Player.RemoveState(State.Turning);

            float GetTargetRotation()
            {
                Quaternion rot = Quaternion.LookRotation(ConvertedDirection, transform.up);
                return rot.eulerAngles.y;
            }
        }
    }
}
