using UnityEngine;

namespace Capstone
{
    public class ParticleSpin : MonoBehaviour
    {
        [SerializeField] private Vector3 spin;
        [SerializeField] private float speed;
        private ParticleSystem particles;

        private void Start()
        {
            particles = GetComponent<ParticleSystem>();
            transform.eulerAngles = Vector3.zero;
        }

        private void Update()
        {
            //particles.shape.rotation += spin * (Time.deltaTime * speed);
        }

        private void OnDrawGizmosSelected()
        {
            //transform.Rotate(spin * (Time.deltaTime * speed));
        }
    }
}