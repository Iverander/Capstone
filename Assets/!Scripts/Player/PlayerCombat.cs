using System;
using UnityEngine;
using MackySoft.SerializeReferenceExtensions.Editor;

namespace Capstone
{
    public class PlayerCombat : MonoBehaviour
    {
        CombatAbility selectedAbility;
        [SerializeReference, SubclassSelector]public CombatAbility[] abilities;

        private void Start()
        {
            foreach (var ability in abilities)
            {
                if(ability == null) continue;
                ability.Initialize(transform);
            }
            
            SetAbiilty(0);
            Player.input.onAttack.AddListener(UseAbility);
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var ability in abilities)
            {
                if(ability == null) continue;
                if (!ability.ShowGizmos) continue;
                
                ability.Gizmos(transform);
            }
        }

        void SetAbiilty(int ability)
        {
            selectedAbility = abilities[ability];
        }

        void UseAbility()
        {
            selectedAbility.Preform();
        }
    }
}
