using Capstone.Datapoints;
using SceneSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] UIDocument pauseDocument;
        [SerializeField] Scene menuScene;
        void Start()
        {
            pauseDocument.rootVisualElement.Q<Button>("Menu").clicked += ExitToMenu;
        }
        void OnDestroy()
        {
            pauseDocument.rootVisualElement.Q<Button>("Menu").clicked -= ExitToMenu;
        }

        void ExitToMenu()
        {
            menuScene.Load();
            
            //Session.active.NewSection("Exit to menu");
        }
    }
}
