using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capstone
{
    [Serializable]
    public abstract class Ability
    {
        [field: SerializeField]public string name { get; private set; }
        protected Creature origin;
        
        [Space, SerializeField] public float cooldown = .2f;
        [field:SerializeField]public bool onCooldown { get; private set; }
        [field: SerializeField] public Color color { get; private set; } = Color.red;
        public Action<float> performed;
        
        [Header("Gizmos")]
        public bool ShowGizmos;
        
        [Header("Art")]
        [SerializeField] protected ParticleSystem effectPrefab;
        
        
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
            if(effectPrefab)
                Effect();
        }
        

        protected abstract void Action();
        protected abstract void Effect();

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
