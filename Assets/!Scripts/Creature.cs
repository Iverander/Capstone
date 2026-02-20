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
        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
        }
    }
}
