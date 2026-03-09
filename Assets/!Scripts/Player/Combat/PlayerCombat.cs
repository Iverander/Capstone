using AYellowpaper.SerializedCollections;
using System;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public enum AbilityKeys
    {
        None = 0,
        MB1 = 1,
        Q = 2,
        E = 3,
        F = 4
    }

    public class PlayerCombat : MonoBehaviour
    {

        [SerializedDictionary] public SerializedDictionary<AbilityKeys, Ability> abilities;

        private void Start()
        {
            foreach (var ability in abilities.Values)
            {
                if (ability == null) continue;
                ability.Initialize(Player.instance);
            }

            Player.input.onAbility.AddListener(UseAbility);
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var ability in abilities.Values)
            {
                if (ability == null) continue;
                if (!ability.ShowGizmos) continue;

                ability.Gizmos(transform);
            }
        }

        void UseAbility(int abilityIndex)
        {
            //Debug.Log("Preforming ability " + abilities[(AbilityKeys)abilityIndex]);
            abilities[(AbilityKeys)abilityIndex].Perform();
        }
    }
}
