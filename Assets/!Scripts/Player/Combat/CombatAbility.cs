using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class CombatAbility : Ability
    {
        
        [Header("Stats")] 
        [SerializeField] protected Vector3 size = Vector3.one;
        [field: SerializeField] protected Vector3 center { get; private set; } = Vector3.up + Vector3.forward;
        protected Vector3 trueCenter => origin.transform.rotation * center + origin.transform.position;

        [Space] [SerializeField] protected bool includeSelf = false;
        [SerializeField] protected float damage = 10;
        [SerializeField] protected float knockbackForce = 5;
        [SerializeField] protected float duration = .3f;

        /// <summary>
        /// Perform the ability
        /// </summary>
        /// <typeparam name="T">The type of creature to hit</typeparam>
        public override void Action<T>()
        {
            if(size == Vector3.zero) return;
            
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
                    creature.health.Damage(damage);
                }

                if (knockbackForce > 0)
                {
                    creature.Knockback(origin.transform.position, knockbackForce, duration);
                }   
            }
        }
        
        public override void Gizmos(Transform origin)
        {
            base.Gizmos(origin);
            if(origin == null) return;
            UnityEngine.Gizmos.DrawWireCube(center, size);
        }
    }
}
