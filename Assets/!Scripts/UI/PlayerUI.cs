using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset abilityDocument;
        private VisualElement root;

        private void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            AddAbilityUI();
        }

        private void AddAbilityUI()
        {
            var abilityContainer = root.Q("AbilityContainer");

            foreach (var ability in Player.instance.combat.abilities)
            {
                if (ability.Value.ability == null) continue;
                VisualElement uiAbility = abilityDocument.Instantiate();
                abilityContainer.Add(uiAbility);
                uiAbility.Q<Label>().text = $"{ability.Key}: {ability.Value.ability.name}";

                var bar = uiAbility.Q<ProgressBar>();
                bar.highValue = ability.Value.ability.cooldown;
                bar.value = bar.highValue;
                bar.Q(className: "unity-progress-bar__progress").style.backgroundColor = ability.Value.ability.color;

                ability.Value.ability.performed += async cooldown =>
                {
                    bar.value = 0;
                    for (float i = 0; i < Mathf.RoundToInt(cooldown * 100); i += Time.timeScale)
                    {
                        await Task.Delay(10);
                        bar.value += .01f * Time.timeScale;
                    }
                };
            }
        }
    }
}