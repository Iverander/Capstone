using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Capstone
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private UIDocument keyIndicator;
        [SerializeField] private UIDocument shopUI;

        private void Start()
        {
            keyIndicator.enabled = false;
            shopUI.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            keyIndicator.enabled = true;
            Player.input.onShop.AddListener(ToggleShop);
        }

        private void OnTriggerExit(Collider other)
        {
            keyIndicator.enabled = false;
            Player.input.onShop.RemoveListener(ToggleShop);
        }
        private void ToggleShop()
        {
            shopUI.enabled = !shopUI.enabled;

            if (shopUI.enabled)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
