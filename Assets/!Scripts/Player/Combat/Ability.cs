using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public abstract class Ability 
    {
        protected Creature origin;
        
        [field: SerializeField] public string name { get; private set; }
        
        [Space, SerializeField] public float cooldown = .2f;
        [field: SerializeField] public bool onCooldown { get; private set; }

        [Header("Gizmos")]
        public bool ShowGizmos;
        [field: SerializeField] protected Color gizmosColor { get; private set; } = Color.red;
        
        public Action<float> performed;
        
        public void Initialize(Creature origin)
        {
            this.origin = origin;
        }

        public void Perform<T>() where T : Creature
        {
            if(onCooldown) return;
            performed?.Invoke(cooldown);
            if(cooldown > 0)
                _=Cooldown();
            
            Action<T>();
        }
        public abstract void Action<T>() where T : Creature;

        protected async Task Cooldown()
        {
            onCooldown = true;
            
            await Task.Delay(Mathf.RoundToInt(cooldown * 1000));
            
            onCooldown = false;
        }
        
        
        public virtual void Gizmos(Transform origin)
        {
            if(origin == null) return;
            UnityEngine.Gizmos.matrix = origin.localToWorldMatrix;
            UnityEngine.Gizmos.color = gizmosColor;
        }
    }
}
