using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class StoreFront : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private List<Modifier> Sellable;
        [SerializeField] private VisualTreeAsset productAsset;
        private UIDocument shopUI;

        private async void Start()
        {
            shopUI = GetComponent<UIDocument>();
            shopUI.rootVisualElement.Q<Label>("PlayerCash").text = Wallet.Cash.ToString();
            Wallet.cashUpdated += RefreshCash;
            Sellable = await Addressable.LoadAssets<Modifier>("Modifier");

            Open();
        }

        public void OnDestroy()
        {
            Wallet.cashUpdated -= RefreshCash;
        }

        public void Open()
        {
            //Time.timeScale = 0;
            //Cursor.lockState = CursorLockMode.Confined;            
            foreach (var mod in Sellable)
            {
                if(mod.gained) continue;
                VisualElement product = productAsset.Instantiate();
                shopUI.rootVisualElement.Q("ProductShelf").Add(product);
                product.Q<Label>().text = mod.name;
                product.Q<Label>("Cost").text = mod.cost.ToString();
                product.Q<Image>().sprite = mod.icon;
                product.Q<Button>().clicked += async () =>
                {
                    if (mod.cost > Wallet.Cash)
                    {
                        product.Q<Label>("Cost").text = "<color=red>too poor</color>";
                        await Task.Delay(1000);
                        product.Q<Label>("Cost").text = mod.cost.ToString();
                        return;
                    }

                    Wallet.TakeCash(mod.cost);
                    Player.instance.modifier.AddModifier(mod);
                    Sellable.Remove(mod);
                    shopUI.rootVisualElement.Q("ProductShelf").Remove(product);
                };
            }
        }

        private void RefreshCash(float amount)
        {
            shopUI.rootVisualElement.Q<Label>("PlayerCash").text = amount.ToString();
        }
    }
}