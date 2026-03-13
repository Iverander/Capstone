using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone
{
    [DefaultExecutionOrder(-100)] //Marie did this btw she's so good at programming and she understands what she's doing completely
    public class Health : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public float health { get; private set; }
        [field: SerializeField] public float maxHealth { get; private set; } = 100;
        
        [SerializeField] float regenerationRate;

        [SerializeField] bool destroyOnDeath;

        [Space]
        public UnityEvent<float> Damaged;
        public UnityEvent Killed;

        private void Start()
        {
            health = maxHealth;
        }

        private void Update()
        {
            if(regenerationRate <= 0) return;
            Heal( regenerationRate * Time.deltaTime);
        }

        public void Damage(float amount)
        {
            health -= amount;
            Damaged?.Invoke(health);

            if (health <= 0)
            {
                Killed?.Invoke();

                if (destroyOnDeath)
                    Destroy(gameObject);
            }
        }

        public void Heal(float amount)
        {
            health += amount;
            
            if(health > maxHealth)
                health = maxHealth;
        }
    }
}
