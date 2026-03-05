using System;
using UnityEngine;

namespace Capstone
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Creature : MonoBehaviour
    {
        public Health health { get; private set; }
        public Rigidbody rb { get; private set; }
        protected bool knockbacked;
        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
        }

        public abstract void Knockback(Vector3 origin, float knockback, float duration);
    }
}
