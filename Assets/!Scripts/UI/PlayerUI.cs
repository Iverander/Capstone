using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class PlayerUI : MonoBehaviour
    {
        private VisualElement root;

        [SerializeField] private VisualTreeAsset abilityDocument;
        
        void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            
            AddAbilityUI();
        }

        void AddAbilityUI()
        {
            VisualElement abilityContainer = root.Q("AbilityContainer");

            foreach (var ability in Player.instance.combat.abilities)
            {
                if(ability.Value.ability == null) continue;
                VisualElement uiAbility = abilityDocument.Instantiate();
                abilityContainer.Add(uiAbility);
                uiAbility.Q<Label>().text = $"{ability.Key}: {ability.Value.ability.name}";
                
                ProgressBar bar = uiAbility.Q<ProgressBar>();
                bar.highValue = ability.Value.ability.cooldown;
                bar.value = bar.highValue;
                bar.Q(className:"unity-progress-bar__progress").style.backgroundColor = ability.Value.ability.color;

                ability.Value.ability.performed += async (cooldown) =>
                {
                    bar.value = 0;
                    for (int i = 0; i < Mathf.RoundToInt(cooldown * 100); i++)
                    {
                        await Task.Delay(10); 
                        bar.value += .01f;
                    }
                };
            }
        }
    }
}
