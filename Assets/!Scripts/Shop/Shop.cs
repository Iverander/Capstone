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
        [SerializeField] GameObject shopCurtain;

        bool shopOpen;

        private void Start()
        {
            keyIndicator.enabled = false;
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

        void OpenStorefront()
        {
            
            MenuManager.OpenMenu(MenuManager.Menu.Store);
        }

        void OpenShop()
        {
            shopOpen = true;
            shopCurtain.SetActive(false);
        }

        public void CloseShop()
        {
            shopOpen=false;
            shopCurtain.SetActive(true);
        }
    }
}
