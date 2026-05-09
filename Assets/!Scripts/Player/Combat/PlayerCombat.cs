using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Capstone
{
    public enum AbilityKeys
    {
        None = 0,
        LMB = 1,
        Q = 2,
        E = 3,
        RMB = 4
    }

    public class PlayerCombat : MonoBehaviour
    {
        [SerializedDictionary] public SerializedDictionary<AbilityKeys, workaround> abilities;

        private void Start()
        {
            foreach (var ability in abilities.Values)
            {
                if (ability.ability == null) continue;
                ability.ability.Initialize(Player.instance);
            }

            Player.input.onAbility.AddListener(UseAbility);
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var ability in abilities.Values)
            {
                if (ability.ability == null) continue;
                if (!ability.ability.ShowGizmos) continue;

                ability.ability.Gizmos(transform);
            }
        }

        private void UseAbility(int abilityIndex)
        {
            //Debug.Log("Preforming ability " + abilities[(AbilityKeys)abilityIndex]);
            abilities[(AbilityKeys)abilityIndex].ability.Perform();
        }

        [Serializable]
        public class workaround
        {
            [SerializeReference] [SubclassSelector]
            public Ability ability;
        }
    }
}