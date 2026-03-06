using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capstone
{
    [Serializable]
    public abstract class Ability : ScriptableObject
    {
        protected Creature origin;
        
        [Space, SerializeField] public float cooldown = .2f;
        public bool onCooldown { get; private set; }
        [field: SerializeField] protected Color color { get; private set; } = Color.red;

        [Header("Gizmos")]
        public bool ShowGizmos;
        
        public Action<float> performed;
        
        public void Initialize(Creature origin)
        {
            this.origin = origin;
        }
        
        public void Perform() 
        {
            if(onCooldown) return;
            performed?.Invoke(cooldown);
            if(cooldown > 0)
                _=Cooldown();
            
            Action();
        }
        public abstract void Action();

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
            UnityEngine.Gizmos.color = color;
        }
    }
}
