using System;
using System.Linq;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class CombatAbility
    {
        protected Transform origin;
        
        [field: SerializeField] public string name { get; private set; }

        [Header("Gizmos")]
        public bool ShowGizmos;
        [field: SerializeField] protected Color gizmosColor { get; private set; } = Color.red;
        
        [Header("Stats")] 
        [SerializeField] protected Vector3 size = Vector3.one;
        [field: SerializeField] protected Vector3 center { get; private set; } = Vector3.up + Vector3.forward;
        protected Vector3 trueCenter => origin.rotation * center + origin.position;
        
        [Space]
        [SerializeField] protected float damage;
        [SerializeField] protected float knockbackForce;

        public void Initialize(Transform origin)
        {
            this.origin = origin;
        }

        public virtual void Preform()
        {
            Collider[] hit = Physics.OverlapBox(trueCenter, size / 2, origin.rotation);
            
            if(hit.Length <= 0) return;

            if (damage > 0)
            {
                foreach (var col in hit)
                {
                    Hurt(col.GetComponent<Health>()); //replace not good to spam getcomponent like this
                }
            }

            if (knockbackForce > 0)
            {
                Knockback(hit, knockbackForce);
            }
        }

        public virtual void Gizmos(Transform origin)
        {
            if(origin == null) return;
            UnityEngine.Gizmos.matrix = origin.localToWorldMatrix;
            UnityEngine.Gizmos.color = gizmosColor;
            UnityEngine.Gizmos.DrawWireCube(center, size);
        }
        
        public virtual void Hurt(Health health)
        {
            health.Damage(damage);   
        }
        public virtual void Knockback(Collider[] toKnockback, float force)
        {
            Collider[] toPush = toKnockback.Where(x => x.attachedRigidbody != null).ToArray();
            if(toPush.Length <= 0) return;
            
            foreach (Collider col in toPush)
            {
                Rigidbody rb = col.attachedRigidbody;
                rb.AddForce((rb.transform.position - origin.position) * force * 10, ForceMode.Force);
            }
        }
    }
}
