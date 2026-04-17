using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Capstone
{
    public class DeathUI : MonoBehaviour
    {
        UIDocument uiDocument;
        [SerializeField] private Scene menuScene;

        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            uiDocument.rootVisualElement.style.display =  DisplayStyle.None;
            uiDocument.rootVisualElement.Q<Button>().clicked += ReturnToMenu;
        }

        private void ReturnToMenu()
        {
            menuScene.Load();
        }

        public void ShowDeathScreen()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined; 
            uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}
