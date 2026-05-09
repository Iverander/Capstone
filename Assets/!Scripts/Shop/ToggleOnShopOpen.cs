using UnityEngine;

namespace Capstone
{
    [DefaultExecutionOrder(-100)]
    public class ToggleOnShopOpen : MonoBehaviour
    {
        [SerializeField] private bool reverse;
        private Shop shop;

        private void Start()
        {
            shop = GetComponentInParent<Shop>();
            shop.onShopToggle += Toggle;
        }

        private void OnDestroy()
        {
            shop.onShopToggle -= Toggle;
        }

        private void Toggle(bool open)
        {
            var toggle = reverse ? open : !open;
            gameObject.SetActive(toggle);
        }
    }
}