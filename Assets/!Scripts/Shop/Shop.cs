using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private UIDocument keyIndicator;
        [SerializeField] private GameObject shopCurtain;

        public Action<bool> onShopToggle;
        private bool shopOpen;

        private void Start()
        {
            keyIndicator.enabled = false;
            CloseShop();
            RoundManager.onBetweenRound.AddListener(OpenShop);
            RoundManager.onNewRound.AddListener(CloseShop);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!shopOpen) return;
            keyIndicator.enabled = true;
            Player.input.onShop.AddListener(OpenStorefront);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!shopOpen) return;
            keyIndicator.enabled = false;
            Player.input.onShop.RemoveListener(OpenStorefront);
        }

        private void OpenStorefront()
        {
            MenuManager.OpenMenu(MenuManager.Menu.Store);
            Player.input.onShop.RemoveListener(OpenStorefront);
            Player.input.onShop.AddListener(CloseStorefront);
        }

        private void CloseStorefront()
        {
            MenuManager.instance.CloseMenu();
            Player.input.onShop.AddListener(OpenStorefront);
            Player.input.onShop.RemoveListener(CloseStorefront);
        }

        private void OpenShop()
        {
            shopOpen = true;
            onShopToggle?.Invoke(true);
        }

        public void CloseShop()
        {
            shopOpen = false;
            onShopToggle?.Invoke(false);
        }
    }
}