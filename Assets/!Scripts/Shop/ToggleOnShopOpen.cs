using System;
using UnityEngine;

namespace Capstone
{
    [DefaultExecutionOrder(-100)]
    public class ToggleOnShopOpen : MonoBehaviour
    {
        Shop shop;
        [SerializeField] private bool reverse;

        private void Start()
        {
            shop = GetComponentInParent<Shop>();
            shop.onShopToggle += Toggle;
        }

        private void OnDestroy()
        {
           shop.onShopToggle -= Toggle; 
        }

        void Toggle(bool open)
        {
            bool toggle = reverse ? open : !open;
            gameObject.SetActive(toggle);
        }
    }
}
