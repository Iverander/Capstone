using UnityEngine;

namespace Capstone
{
    [CreateAssetMenu(fileName = "MaxHealthModifier", menuName = "Scriptable Objects/Modifier/MaxHealth")]
    public class HealthMod : Modifier
    {
        [SerializeField] private float maxHealth;

        public override void onGained()
        {
            base.onGained();
            Player.instance.health.AddMaxHealth(maxHealth);
        }
    }
}