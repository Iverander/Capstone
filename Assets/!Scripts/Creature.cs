using System;
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
        protected bool stunned;
        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            stunEffect.SetActive(false);
        }

        public abstract void Stun(float duration);
    }
}
