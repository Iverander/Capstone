using System;
using SceneSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

//using Scene = UnityEngine.SceneManagement.Scene;

namespace Capstone
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UIDocument mainMenu;

        [SerializeField] private SceneGroup gameScene;
        //[SerializeField] Scene playerScene;

        private Button gameSceneButton;
        private EnumField mapField;
        private Toggle obstacleToggle;
        private Button quitButton;
        private Button randomizeButton;

        private VisualElement root;
        private EnumField shaderField;
        private EnumField weatherField;

        private void Start()
        {
            DataManager.SaveSessions();
            MapManager.Instance.currentMap = null;

            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Confined;
            root = mainMenu.rootVisualElement;

            gameSceneButton = root.Q<Button>("Start");
            quitButton = root.Q<Button>("Quit");
            weatherField = root.Q<EnumField>("WeatherSelector");
            shaderField = root.Q<EnumField>("ShaderSelector");
            mapField = root.Q<EnumField>("MapSelector");
            obstacleToggle = root.Q<Toggle>("ObstacleToggle");
            randomizeButton = root.Q<Button>("Randomize");

            weatherField.value = Settings.active.mapSettings.weatherType;
            shaderField.value = Settings.active.shaderType;
            mapField.value = Settings.active.mapSettings.map;
            obstacleToggle.value = Settings.active.mapSettings.obstacles;

            gameSceneButton.clicked += StartGame;
            quitButton.clicked += Application.Quit;
            randomizeButton.clicked += RandomizeSettings;

            weatherField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                Settings.active.mapSettings.SetWeather((WeatherType)changeEvent.newValue);
            });
            shaderField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                Settings.active.shaderType = (ShaderType)changeEvent.newValue;
                //Debug.Log(LevelSettings.shaderType);
            });
            mapField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                Settings.active.mapSettings.map = (Map)changeEvent.newValue;
            });

            obstacleToggle.RegisterCallback<ChangeEvent<bool>>(changeEvent =>
            {
                Settings.active.mapSettings.ToggleObstacles(changeEvent.newValue);
            });
        }

        private void OnDestroy()
        {
            gameSceneButton.clicked -= StartGame;
            quitButton.clicked -= Application.Quit;
            randomizeButton.clicked -= RandomizeSettings;
        }

        private void StartGame()
        {
            gameSceneButton.SetEnabled(false);
            gameSceneButton.text = "Loading";

            gameScene.Load();
            MapManager.LoadMap();
            //playerScene.Load();
        }

        private void RandomizeSettings()
        {
            Settings.active.Randomize();

            weatherField.value = Settings.active.mapSettings.weatherType;
            shaderField.value = Settings.active.shaderType;
            mapField.value = Settings.active.mapSettings.map;
            obstacleToggle.value = Settings.active.mapSettings.obstacles;

            randomizeButton.text = "Randomize Successful";
        }
    }
}