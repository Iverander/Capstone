using System;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public abstract class CombatAbility
    {
        private Transform origin;
        [field: SerializeField] public bool ShowGizmos { get; private set; }

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
