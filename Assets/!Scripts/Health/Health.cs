using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone
{
    [DefaultExecutionOrder(-100), RequireComponent(typeof(Creature))] //Marie did this btw she's so good at programming and she understands what she's doing completely
    public class Health : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public float health { get; private set; }
        [field: SerializeField] public float maxHealth { get; private set; } = 100;
        
        [SerializeField] float regenerationRate;
        private bool regenerate = true;

        [SerializeField] bool destroyOnDeath;
        
        [Space]
        public UnityEvent<float> Damaged;
        public UnityEvent Killed;
        public Action<float> healthChanged; 
        public Action<float> maxHealthChanged;
        
        Creature creature;

        private void Start()
        {
            health = maxHealth;
            creature = GetComponent<Creature>();
        }

        private void Update()
        {
            if(regenerationRate <= 0 || !regenerate) return;
            Heal( regenerationRate * (maxHealth / 100) * Time.deltaTime);
        }
        
        Coroutine regenerationRoutine;
        IEnumerator StopRegeneration(float time = 5)
        {
            regenerate = false;
            yield return new WaitForSeconds(time);
            regenerate = true;
            regenerationRoutine = null;
        }
        
        public void Damage(float amount)
        {
            if(regenerationRoutine != null)
                StopCoroutine(regenerationRoutine);
            
            health -= amount / creature.stats.DamageResistance;
            Damaged?.Invoke(health);
            healthChanged?.Invoke(health);
            
            regenerationRoutine = StartCoroutine(StopRegeneration());

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
            
            healthChanged?.Invoke(health);
        }

        public void AddMaxHealth(float amount)
        {
            maxHealth += amount;
            maxHealthChanged?.Invoke(maxHealth);
        }
    }
}
