using System;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class MovementAbility : Ability
    {
        [Header("Stats")]
        [SerializeField] protected float force;
        
        [InfoBox("Zero is adaptive :)")]
        [SerializeField] Vector3 direction;

        [SerializeField] private float duration = .15f;
        protected override void Action()
        {
            Vector3 directionVector = direction;
            if (directionVector == Vector3.zero)
            {
                directionVector = Player.instance.movement.ConvertedDirection;
            }

            //origin.
            origin.StartCoroutine(origin.Stun(duration));

            if(directionVector.normalized != Vector3.zero) 
                origin.rb.AddForce(directionVector.normalized * (force), ForceMode.Impulse);
            else
                origin.rb.AddForce(origin.transform.forward * (force), ForceMode.Impulse);
        }

        protected override void Effect(Vector3 offset)
        {
            base.Effect(Vector3.up + origin.transform.position);
        }
    }
}
