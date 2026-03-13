using System;
using UnityEngine;

namespace Capstone
{
    public class ParticleSpin : MonoBehaviour
    {
        ParticleSystem particles;
        [SerializeField] private Vector3 spin;
        [SerializeField] private float speed;

        private void Start()
        {
            particles = GetComponent<ParticleSystem>();
            transform.eulerAngles = Vector3.zero;
        }

        void Update()
        {
             //particles.shape.rotation += spin * (Time.deltaTime * speed);
        }

        private void OnDrawGizmosSelected()
        {
            //transform.Rotate(spin * (Time.deltaTime * speed));
        }
    }
}
