using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone
{
    public class Health : MonoBehaviour
    {
        [field: SerializeField, ReadOnly]public float health { get; private set; }
        [field: SerializeField] public float maxHealth { get; private set; } = 100;
        
        [SerializeField] bool destroyOnDeath;

        [Space]
        public UnityEvent Damaged;
        public UnityEvent Killed;

        private void Start()
        {
            health = maxHealth;
        }

        public void Damage(float amount)
        {
            health -= amount;
            Damaged?.Invoke();
            
            if (health <= 0)
            {
                Killed?.Invoke();
                
                if(destroyOnDeath)
                    Destroy(gameObject);
            }
        }
    }
}
