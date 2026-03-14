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

        public static Action<float> cashUpdated;
        public static Wallet Instance { get; private set; }

        private void Start()
        {
            Instance = this;

#if  !UNITY_EDITOR
      _cash = 0;      
#endif
        }

        public static void GiveCash(int amount)
        {
            Cash += amount;
            cashUpdated?.Invoke(Cash);
        }

        public static void TakeCash(int amount)
        {
            Cash -= amount;
            cashUpdated?.Invoke(Cash);
        }
    }
}
