using System;
using UnityEngine;

namespace Capstone
{
    [Serializable]
    public class Stats
    {
        public float DamageMultiplier = 1;
        public float DamageResistance = 1;

        public void ModifyDamageMultiplier(float multiplier)
        {
            DamageMultiplier += multiplier;
            Debug.Log("DamageMultiplier");
        }

        public void ModifyDamageResistance(float multiplier)
        {
            DamageResistance += multiplier;
        }
    }
}
