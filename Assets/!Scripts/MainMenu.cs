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

            weatherField.value = Settings.mapSettings.weatherType;
            shaderField.value = Settings.shaderType;
            mapField.value = Settings.mapSettings.map;
            obstacleToggle.value = Settings.mapSettings.obstacles;

            gameSceneButton.clicked += StartGame;
            quitButton.clicked += Application.Quit;
            
            weatherField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                Settings.mapSettings.SetWeather((WeatherType)changeEvent.newValue);
            });
            shaderField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                Settings.shaderType = (ShaderType)changeEvent.newValue;
                //Debug.Log(LevelSettings.shaderType);
            });
            mapField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                Settings.mapSettings.map = (Map)changeEvent.newValue;
            });
            
            obstacleToggle.RegisterCallback<ChangeEvent<bool>>(changeEvent =>
            {
                Settings.mapSettings.ToggleObstacles(changeEvent.newValue);
            });
        }

        void OnDestroy()
        {
            gameSceneButton.clicked -= StartGame;
            quitButton.clicked -= Application.Quit;
        }

        void StartGame()
        {
            gameSceneButton.SetEnabled(false);
            gameSceneButton.text = "Loading";

            gameScene.Load();
            GetMap().Load();
            //playerScene.Load();
        }

        Scene GetMap()
        {
            switch(Settings.shaderType)
            {
                case ShaderType.HLSL:   
                    return HLSLMaps[Settings.mapSettings.map];
                case ShaderType.ShaderGraph:
                    return SGMaps[Settings.mapSettings.map];
            }

            return null;
        }
    }
}
