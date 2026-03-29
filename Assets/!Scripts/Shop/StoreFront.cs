using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Capstone
{
    public class StoreFront : MonoBehaviour
    {
        [SerializeField, ReadOnly] private List<Modifier> Sellable;
        private UIDocument shopUI;
        [SerializeField] private VisualTreeAsset productAsset;

        async void Start()
        {
            shopUI = GetComponent<UIDocument>();
            shopUI.enabled = false;
            Sellable = await Addressable.LoadAssets<Modifier>("Modifier");
        }

        public void Toggle()
        {
            shopUI.enabled = !shopUI.enabled;

            if (shopUI.enabled)
                Open();
            else
                Close();
        }

        public void Open()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            shopUI.rootVisualElement.Q<Label>("PlayerCash").text = Wallet.Cash.ToString();
            Wallet.cashUpdated += RefreshCash;
            
            foreach (Modifier mod in Sellable)
            {
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

        public void Close()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Wallet.cashUpdated -= RefreshCash;
        }
    }
}
