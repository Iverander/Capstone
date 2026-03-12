using System;
using UnityEngine;

namespace Capstone
{
    public class Spin : MonoBehaviour
    {
        [SerializeField] private Vector3 spin;
        [SerializeField] private float speed;

        private void Start()
        {
            transform.eulerAngles = Vector3.zero;
        }

        void Update()
        {
            transform.Rotate(spin * (Time.deltaTime * speed));
        }

        private void OnDrawGizmosSelected()
        {
            transform.Rotate(spin * (Time.deltaTime * speed));
        }
    }
}
