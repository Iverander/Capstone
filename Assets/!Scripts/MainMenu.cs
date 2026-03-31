using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
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

        [SerializeField, SerializedDictionary] SerializedDictionary<Map, Scene> HLSLMaps = new();
        [SerializeField, SerializedDictionary] SerializedDictionary<Map, Scene> SGMaps = new();  

        Button gameSceneButton;
        Button showcaseButton;
        Button quitButton;
        Label descriptionLabel;
        EnumField weatherField;
        EnumField shaderField;
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
            shaderField = root.Q<EnumField>("ShaderSelector");
            obstacleToggle = root.Q<Toggle>("ObstacleToggle");

            weatherField.value = LevelSettings.CurrentMapSettings.weatherType;
            shaderField.value = LevelSettings.shaderType;
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

            gameSceneButton.clicked += () => StartGame();
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
            weatherField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                LevelSettings.shaderType = (ShaderType)changeEvent.newValue;
                Debug.Log(LevelSettings.shaderType);
            });
            
            obstacleToggle.RegisterCallback<ChangeEvent<bool>>(changeEvent =>
            {
                LevelSettings.ToggleObstacles(changeEvent.newValue);
            });
        }

        void StartGame()
        {
            gameSceneButton.SetEnabled(false);
            gameSceneButton.text = "Loading";

            StartCoroutine(gameScene.Load());
            StartCoroutine(GetMap().Load());
            StartCoroutine(playerScene.Load());
        }

        Scene GetMap()
        {
            switch(LevelSettings.shaderType)
            {
                case ShaderType.HLSL:
                    return HLSLMaps[Map.Mountain];
                case ShaderType.ShaderGraph:
                    return HLSLMaps[Map.Mountain];
            }

            return null;
        }
    }
}
