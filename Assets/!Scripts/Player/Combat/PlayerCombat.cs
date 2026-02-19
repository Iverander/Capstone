using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

namespace Capstone
{
    public enum AbilityKeys
    {
        None = 0,
        M1 = 1,
        Q = 2,
        E = 3,
        F = 4
    }

    public class PlayerCombat : MonoBehaviour
    {
        [Serializable]
        public class Ability //workaround lol - lol
        {
            [SerializeReference, SubclassSelector] public CombatAbility ability;
        }

        [SerializedDictionary] public SerializedDictionary<AbilityKeys, Ability> abilities;

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

        void UseAbility(int abilityIndex)
        {
            Debug.Log("Preforming ability " + abilities[(AbilityKeys)abilityIndex].ability);
            abilities[(AbilityKeys)abilityIndex].ability.Perform<Enemy>();
        }
    }
}
