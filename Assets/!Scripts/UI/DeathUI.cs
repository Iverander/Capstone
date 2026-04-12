using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class DeathUI : MonoBehaviour
    {
        UIDocument uiDocument;

        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            uiDocument.rootVisualElement.style.display =  DisplayStyle.None; 
        }

        public void ShowDeathScreen()
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}
