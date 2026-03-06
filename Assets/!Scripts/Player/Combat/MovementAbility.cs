using System;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    [CreateAssetMenu(fileName = "MovementAbility", menuName = "Scriptable Objects/MovementAbility")]
    public class MovementAbility : Ability
    {
        [Header("Stats")]
        [SerializeField] protected float force;
        
        [InfoBox("Zero is adaptive :)")]
        public Vector3 direction;
        public override void Action()
        {
            Vector3 directionVector = direction;
            if (directionVector == Vector3.zero)
            {
                directionVector.x = origin.rb.linearVelocity.x;
                directionVector.z = origin.rb.linearVelocity.z;
            }

            //origin.

            origin.rb.AddForce(directionVector.normalized * (10 * force), ForceMode.Impulse);
        }
    }
}
