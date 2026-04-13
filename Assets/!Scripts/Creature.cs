using System;
using System.Collections;
using UnityEngine;

namespace Capstone
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Creature : MonoBehaviour
    {
        [SerializeField] protected GameObject stunEffect;
        public Health health { get; private set; }
        public Rigidbody rb { get; private set; }
        public bool stunned { get; protected set; }
        [field: SerializeField] public Animator animator { get; private set; }
        public Stats stats;

        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            stunEffect.SetActive(false);
        }

        public abstract IEnumerator Stun(float durationSeconds);
    }
}
