using UnityEngine;

namespace Capstone
{
    [CreateAssetMenu(fileName = "AttackModifier", menuName = "Scriptable Objects/Modifier/Attack")]
    public class AttackMod : Modifier
    {
        [SerializeField] private float additionalDamage;
        public override void onGained()
        {
            Player.instance.stats.ModifyDamageMultiplier(additionalDamage);
        }
    }
}
