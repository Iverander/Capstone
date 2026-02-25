using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
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

        private Button gameSceneButton;
        private Button showcaseButton;
        Label descriptionLabel;
        private void Start()
        {
            root = mainMenu.rootVisualElement;
            
            gameSceneButton = root.Q<Button>("GameScene");
            showcaseButton = root.Q<Button>("Showcase");
            descriptionLabel = root.Q<Label>("Description");

            gameSceneButton.RegisterCallback<MouseOverEvent> ((mouseEvent) =>
            {
                descriptionLabel.text = "Game scene is the game itself, only things that are finished will be added to this scene.";
            });
            showcaseButton.RegisterCallback<MouseOverEvent> ((mouseEvent) =>
            {
                descriptionLabel.text = "Showcase is used to display work and progress on unfinished shaders & models.";
            });

            gameSceneButton.clicked += () =>
            {
                SceneManager.LoadScene(gameScene);
            };
            showcaseButton.clicked += () =>
            {
                SceneManager.LoadScene(showcaseScene);
            };
        }

    }
}
