using System;
using UnityEngine;

namespace Capstone
{
    public class Wallet : MonoBehaviour
    {
        [field: SerializeField] public int _cash { get; private set; }

        public static int Cash
        {
            get => Instance._cash;
            set => Instance._cash = value;
        }
        public static Wallet Instance { get; private set; }

        private void Start()
        {
            Instance = this;
        }

        public static void GiveCash(int amount)
        {
            Cash += amount;
        }

        public static void TakeCash(int amount)
        {
            Cash -= amount;
        }
    }
}
