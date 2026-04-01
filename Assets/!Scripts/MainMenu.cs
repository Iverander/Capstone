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

            weatherField.value = LevelSettings.CurrentMapSettings.weatherType;
            shaderField.value = LevelSettings.shaderType;
            mapField.value = LevelSettings.currentMap;
            obstacleToggle.value = LevelSettings.CurrentMapSettings.obstacles;

            gameSceneButton.clicked += StartGame;
            quitButton.clicked += Application.Quit;
            
            weatherField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                LevelSettings.ChangeCurrentWeather((WeatherType)changeEvent.newValue);
            });
            shaderField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                LevelSettings.shaderType = (ShaderType)changeEvent.newValue;
                //Debug.Log(LevelSettings.shaderType);
            });
            mapField.RegisterCallback<ChangeEvent<Enum>>(changeEvent =>
            {
                LevelSettings.currentMap = (Map)changeEvent.newValue;
            });
            
            obstacleToggle.RegisterCallback<ChangeEvent<bool>>(changeEvent =>
            {
                LevelSettings.ToggleObstacles(changeEvent.newValue);
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
            switch(LevelSettings.shaderType)
            {
                case ShaderType.HLSL:
                    return HLSLMaps[LevelSettings.currentMap];
                case ShaderType.ShaderGraph:
                    return SGMaps[LevelSettings.currentMap];
            }

            return null;
        }
    }
}
