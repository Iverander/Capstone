using UnityEngine;
using MackySoft.SerializeReferenceExtensions.Editor;

namespace Capstone
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]public CombatAbility[] abilities;
    }
}
