using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class CombatAbility : Ability
    {
        [SerializeField] private EventReference hitSFX;
        
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
        
        protected override void Action()
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
            
            if(hit.Count <= 0) return;
            AudioManager.PlayOneShot(hitSFX, trueCenter);

            foreach (var creature in hit)
            {
                if (damage > 0)
                {
                    creature.health.Damage(damage * origin.stats.DamageMultiplier);
                }

                if (knockbackForce > 0)
                {
                    creature.StartCoroutine(creature.Stun(duration));
                    creature.rb.AddForce((creature.transform.position - origin.transform.position) * (knockbackForce * 10), ForceMode.Force);
                }   
            }
        }

        protected override void Effect(Vector3 offeset)
        {
            base.Effect(trueCenter);
        }

        public override void Gizmos(Transform origin)
        {
            base.Gizmos(origin);
            if(origin == null) return;
            UnityEngine.Gizmos.DrawWireCube(center, size);
        }
    }
}
