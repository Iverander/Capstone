using System;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public abstract class CombatAbility
    {
        protected Transform origin;
        [field: SerializeField] public bool ShowGizmos { get; private set; }
        
        [Header("Stats")]
        [SerializeField] protected Vector3 center = Vector3.up +Vector3.forward;
        [SerializeField] protected Vector3 size = Vector3.one;

        public void Initialize(Transform origin)
        {
            this.origin = origin;
        }
        
        public abstract void Preform();

        public virtual void Gizmos(Transform origin)
        {
            if(origin == null) return;
            UnityEngine.Gizmos.matrix = origin.localToWorldMatrix;
        }
    }
}
