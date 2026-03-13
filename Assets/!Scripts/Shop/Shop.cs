using System;
using System.Collections.Generic;
using Capstone.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Capstone
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private UIDocument keyIndicator;
        [SerializeField] private StoreFront storeFront;

        private void Start()
        {
            keyIndicator.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            keyIndicator.enabled = true;
            Player.input.onShop.AddListener(storeFront.Toggle);
        }

        private void OnTriggerExit(Collider other)
        {
            keyIndicator.enabled = false;
            Player.input.onShop.RemoveListener(storeFront.Toggle);
            storeFront.Close();
        }
    }
}
