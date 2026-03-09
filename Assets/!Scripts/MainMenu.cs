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

        Button gameSceneButton;
        Button showcaseButton;
        Label descriptionLabel;
        EnumField weatherField;
        Toggle obstacleToggle;
        private void Start()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            root = mainMenu.rootVisualElement;
            
            gameSceneButton = root.Q<Button>("GameScene");
            showcaseButton = root.Q<Button>("Showcase");
            descriptionLabel = root.Q<Label>("Description");
            weatherField = root.Q<EnumField>("WeatherSelector");
            obstacleToggle = root.Q<Toggle>("ObstacleToggle");

            weatherField.value = LevelSettings.CurrentMapSettings.weatherType;
            obstacleToggle.value = LevelSettings.CurrentMapSettings.obstacles;

            gameSceneButton.RegisterCallback<MouseOverEvent> (mouseEvent =>
            {
                descriptionLabel.text = "Game scene is the game itself, only things that are finished will be added to this scene.";
            });
            showcaseButton.RegisterCallback<MouseOverEvent> (mouseEvent =>
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
            
            weatherField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                LevelSettings.ChangeCurrentWeather((WeatherType)changeEvent.newValue);
            });
            
            obstacleToggle.RegisterCallback<ChangeEvent<bool>>(changeEvent =>
            {
                LevelSettings.ToggleObstacles(changeEvent.newValue);
            });
        }

    }
}
