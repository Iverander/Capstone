using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using SceneSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
//using Scene = UnityEngine.SceneManagement.Scene;

namespace Capstone
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UIDocument mainMenu;
        
        VisualElement root;

        [SerializeField] SceneGroup gameScene;
        //[SerializeField] Scene playerScene;

        [SerializeField, SerializedDictionary] SerializedDictionary<Map, Scene> HLSLMaps = new();
        [SerializeField, SerializedDictionary] SerializedDictionary<Map, Scene> SGMaps = new();  

        Button gameSceneButton;
        Button quitButton;
        EnumField weatherField;
        EnumField shaderField;
        EnumField mapField;
        Toggle obstacleToggle;
        Button randomizeButton;
        private void Start()
        {
            Time.timeScale = 1;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
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

        void OnDestroy()
        {
            gameSceneButton.clicked -= StartGame;
            quitButton.clicked -= Application.Quit;
            randomizeButton.clicked -= RandomizeSettings;
        }

        void StartGame()
        {
            gameSceneButton.SetEnabled(false);
            gameSceneButton.text = "Loading";

            gameScene.Load();
            GetMap().Load();
            //playerScene.Load();
        }

        void RandomizeSettings()
        {
            Settings.active.Randomize();
           
            weatherField.value = Settings.active.mapSettings.weatherType;
            shaderField.value = Settings.active.shaderType;
            mapField.value = Settings.active.mapSettings.map;
            obstacleToggle.value = Settings.active.mapSettings.obstacles;
            
            randomizeButton.text = "Randomize Successful";
        }

        Scene GetMap()
        {
            switch(Settings.active.shaderType)
            {
                case ShaderType.HLSL:   
                    return HLSLMaps[Settings.active.mapSettings.map];
                case ShaderType.ShaderGraph:
                    return SGMaps[Settings.active.mapSettings.map];
            }

            return null;
        }
    }
}
