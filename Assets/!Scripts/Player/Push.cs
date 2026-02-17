using System;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class Push : CombatAbility
    {
        [SerializeField] private float force;
        public override void Preform()
        {
            Debug.Log("Preform Push");
            
            Collider[] toPush = Physics.OverlapBox(center + origin.position, size / 2);
            
            if(toPush.Length <= 0) return;

            foreach (Collider col in toPush)
            {
                Rigidbody rb = col.attachedRigidbody;
                if(rb == null) continue;
                
                Debug.Log("Pushing " + col.gameObject.name);
                rb.AddForce((rb.transform.position - origin.position) * force, ForceMode.Force);
            }
        }

        public override void Gizmos(Transform origin)
        {
            base.Gizmos(origin);
            
            UnityEngine.Gizmos.color = Color.yellow;
            UnityEngine.Gizmos.DrawCube(center, size);
        }
    }
}
