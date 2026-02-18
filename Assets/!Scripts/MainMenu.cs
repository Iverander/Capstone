using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Capstone
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UIDocument mainMenu;
        
        VisualElement root;

        [SerializeField, Scene] string gameScene;
        [SerializeField, Scene] string showcaseScene;
        private void Start()
        {
            root = mainMenu.rootVisualElement;

            root.Q<Button>("GameScene").clicked += () =>
            {
                SceneManager.LoadScene(gameScene);
            };
            root.Q<Button>("Showcase").clicked += () =>
            {
                SceneManager.LoadScene(showcaseScene);
            };
        }
    }
}
