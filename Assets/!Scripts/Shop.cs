using System;
using UnityEngine;

namespace Capstone
{
    public class Shop : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
        }

        private void OnTriggerExit(Collider other)
        {
            
        }
    }
}
