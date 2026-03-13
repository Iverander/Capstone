using System;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Capstone
{
    [Serializable]
    public class MovementAbility : Ability
    {
        [Header("Stats")]
        [SerializeField] protected float force;
        
        [InfoBox("Zero is adaptive :)")]
        public Vector3 direction;
        protected override void Action()
        {
            Vector3 directionVector = direction;
            if (directionVector == Vector3.zero)
            {
                directionVector = Player.instance.movement.ConvertedDirection;
            }

            //origin.
            origin.Stun(.15f);

            if(directionVector.normalized != Vector3.zero) 
                origin.rb.AddForce(directionVector.normalized * (force), ForceMode.Impulse);
            else
                origin.rb.AddForce(origin.transform.forward * (force), ForceMode.Impulse);
        }

        protected override void Effect()
        {
            var effect = Object.Instantiate(effectPrefab, origin.transform);
            Object.Destroy(effect.gameObject, effect.main.duration * 2);
        }
    }
}
