using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
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

        [SerializeField] Scene gameScene;
        [SerializeField] Scene playerScene;
        [SerializeField] Scene showcaseScene;

        Button gameSceneButton;
        Button showcaseButton;
        Button quitButton;
        Label descriptionLabel;
        EnumField weatherField;
        Toggle obstacleToggle;
        private void Start()
        {
            Time.timeScale = 1;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            root = mainMenu.rootVisualElement;
            
            gameSceneButton = root.Q<Button>("Start");
            showcaseButton = root.Q<Button>("Showcase");
            quitButton = root.Q<Button>("Quit");
            descriptionLabel = root.Q<Label>("Description");
            weatherField = root.Q<EnumField>("WeatherSelector");
            obstacleToggle = root.Q<Toggle>("ObstacleToggle");

            weatherField.value = LevelSettings.CurrentMapSettings.weatherType;
            obstacleToggle.value = LevelSettings.CurrentMapSettings.obstacles;

            gameSceneButton.RegisterCallback<MouseOverEvent> (mouseEvent =>
            {
                descriptionLabel.text = "Start the game! remember to set your game setting before going in!";
            });
            showcaseButton.RegisterCallback<MouseOverEvent> (mouseEvent =>
            {
                descriptionLabel.text = "Showcase is used to display work and progress on unfinished shaders & models.";
            });
            quitButton.RegisterCallback<MouseOverEvent> (mouseEvent =>
            {
                descriptionLabel.text = "Quit the game";
            });

            gameSceneButton.clicked += () => StartCoroutine(StartGame());
            showcaseButton.clicked += () =>
            {
                //yield return StartCoroutine(showcaseScene.Load());
                //showcaseScene.Activate();
            };
            quitButton.clicked += Application.Quit;
            
            weatherField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                LevelSettings.ChangeCurrentWeather((WeatherType)changeEvent.newValue);
            });
            
            obstacleToggle.RegisterCallback<ChangeEvent<bool>>(changeEvent =>
            {
                LevelSettings.ToggleObstacles(changeEvent.newValue);
            });
        }

        IEnumerator StartGame()
        {
            gameSceneButton.SetEnabled(false);
            gameSceneButton.text = "Loading";

            yield return StartCoroutine(gameScene.Load());
            yield return StartCoroutine(playerScene.Load());
        }

    }
}
