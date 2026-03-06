using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    [CreateAssetMenu(fileName = "CombatAbility", menuName = "Scriptable Objects/CombatAbility")]
    public class CombatAbility : Ability
    {
        
        [Header("Targeting")] 
        [SerializeField] protected Vector3 size = Vector3.one;
        [field: SerializeField] protected Vector3 center { get; private set; } = Vector3.up + Vector3.forward;
        protected Vector3 trueCenter => origin.transform.rotation * center + origin.transform.position;
        [Space]
        [SerializeField] private LayerMask layerToHit;
        [SerializeField] protected bool includeSelf = false;
        
        [Header("Stats")]
        [SerializeField] protected float damage = 10;
        [SerializeField] protected float knockbackForce = 5;
        [SerializeField] protected float duration = .3f;
        
        public override void Action()
        {
            if(size == Vector3.zero) return;
            
            var colliders = Physics.OverlapBox(trueCenter, size / 2, origin.transform.rotation, layerToHit);
            if(colliders.Length <= 0) return;
            
            List<Creature> hit = new();

            foreach (var col in colliders)
            {
                Debug.Log(col.gameObject.name);
                if (col.TryGetComponent(out Creature c))
                {
                    if(includeSelf && c == origin)
                        continue;
                    hit.Add(c);   
                }
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
