using System;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class Push : CombatAbility
    {
        public override void Preform()
        {
            Debug.Log("Preform Push");
        }

        public override void Gizmos(Transform origin)
        {
            base.Gizmos(origin);
            
            UnityEngine.Gizmos.color = Color.yellow;
            UnityEngine.Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}
