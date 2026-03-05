using System;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class MovementAbility : Ability
    {
        [Header("Stats")]
        [SerializeField] protected float distance;
        [SerializeField] protected float time;
        public override void Action<T>()
        {
            //origin.Knockback(-origin.transform.forward + origin.transform.position, strength, 5f);
        }
    }
}
