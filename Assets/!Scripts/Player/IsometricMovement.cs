using UnityEngine;

namespace Capstone
{
    public class IsometricMovement : PlayerMovement
    {
        protected override void Start()
        {
            base.Start();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        protected override void Movement()
        {
            rb.AddForce(100 * currentSpeed * Time.fixedDeltaTime * ConvertedDirection, ForceMode.Force);
            
            LimitSpeed();
        }
    }
}
