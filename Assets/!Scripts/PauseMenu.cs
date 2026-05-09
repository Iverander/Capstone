using SceneSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private UIDocument pauseDocument;
        [SerializeField] private Scene menuScene;

        private void Start()
        {
            pauseDocument.rootVisualElement.Q<Button>("Menu").clicked += ExitToMenu;
        }

        private void OnDestroy()
        {
            //pauseDocument.rootVisualElement.Q<Button>("Menu").clicked -= ExitToMenu;
        }

        private void ExitToMenu()
        {
            Modifier.reset?.Invoke();
            menuScene.Load();

            //Session.active.NewSection("Exit to menu");
        }
    }
}