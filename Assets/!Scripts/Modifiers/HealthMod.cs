using UnityEngine;

namespace Capstone
{
    [CreateAssetMenu(fileName = "MaxHealthModifier", menuName = "Scriptable Objects/Modifier/MaxHealth")]
    public class HealthMod : Modifier
    {
        [SerializeField] float maxHealth;
        public override void onGained()
        {
            Player.instance.health.AddMaxHealth(maxHealth);
        }
    }
}
