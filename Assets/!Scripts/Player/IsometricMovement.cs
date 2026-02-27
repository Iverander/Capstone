using System.Collections;
using UnityEngine;

namespace Capstone
{
    public class IsometricMovement : PlayerMovement
    {
        //protected Vector3 ConvertedDirection => Quaternion.AngleAxis(45, Vector3.up) * moveDirection;
        protected Vector3 ConvertedDirection => transform.forward * moveDirection.z + transform.right * moveDirection.x;
        private Vector3 worldMousePosition;
        

        protected override void Start()
        {
            base.Start();
            Cursor.lockState = CursorLockMode.Confined;
            Player.input.onMousePosition.AddListener(GetMousePosition);
        }

        private void GetMousePosition(Vector2 arg0)
        {
            Ray ray = Player.instance.cam.ScreenPointToRay(arg0);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                worldMousePosition = hit.point;
            }
        }

        protected override void Movement()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            LimitSpeed();
            
            if(!turning)
            {
                //StartCoroutine(FaceWalkDirection(true));
                StartCoroutine(FaceMouseDirection(false));
            }
        }

        IEnumerator FaceMouseDirection(bool basedOnMovement)
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
                Quaternion rot = Quaternion.LookRotation(worldMousePosition, transform.up);
                return rot.eulerAngles.y;
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
