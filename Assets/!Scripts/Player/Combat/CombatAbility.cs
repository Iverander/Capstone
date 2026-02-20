using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class CombatAbility
    {
        protected Creature origin;
        
        [field: SerializeField] public string name { get; private set; }

        [Header("Gizmos")]
        public bool ShowGizmos;
        [field: SerializeField] protected Color gizmosColor { get; private set; } = Color.red;
        
        [Header("Stats")] 
        [SerializeField] protected Vector3 size = Vector3.one;
        [field: SerializeField] protected Vector3 center { get; private set; } = Vector3.up + Vector3.forward;
        protected Vector3 trueCenter => origin.transform.rotation * center + origin.transform.position;
        
        [Space]
        [SerializeField] protected float damage;
        [SerializeField] protected float knockbackForce;

        public void Initialize(Creature origin)
        {
            this.origin = origin;
        }

        /// <summary>
        /// Perform the ability
        /// </summary>
        /// <typeparam name="T">The type of creature to hit</typeparam>
        public virtual void Perform<T>(bool includeSelf = false) where T : Creature
        {
            List<Collider> colliders = Physics.OverlapBox(trueCenter, size / 2, origin.transform.rotation).ToList();
            if(colliders.Count <= 0) return;
            
            List<T> hit = new();

            foreach (var col in colliders)
            {
                if(col.TryGetComponent(out T c))
                    hit.Add(c);
            }

            if (!includeSelf && origin is T origin1)
            {
                hit.Remove(origin1);  
            }

            foreach (var creature in hit)
            {
                if (damage > 0)
                {
                    Hurt(creature.health);
                }

                if (knockbackForce > 0)
                {
                    Knockback(creature.rb, knockbackForce);
                }   
            }
        }
        
        public virtual void Hurt(Health creature)
        {
            creature.Damage(damage);   
        }
        public virtual void Knockback(Rigidbody rb, float force)
        {
            rb.AddForce((rb.transform.position - origin.transform.position) * (force * 10), ForceMode.Force);
        }
        
        public virtual void Gizmos(Transform origin)
        {
            if(origin == null) return;
            UnityEngine.Gizmos.matrix = origin.localToWorldMatrix;
            UnityEngine.Gizmos.color = gizmosColor;
            UnityEngine.Gizmos.DrawWireCube(center, size);
        }
    }
}
