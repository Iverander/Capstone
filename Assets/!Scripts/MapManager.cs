using System;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using SceneSystem;
using UnityEngine;

namespace Capstone
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField, SerializedDictionary]  SerializedDictionary<Map, Scene> HLSLMaps = new();
        [SerializeField, SerializedDictionary] SerializedDictionary<Map, Scene> SGMaps = new();  
        public static MapManager Instance;

        public Scene currentMap;

        private void Start()
        {
            Instance = this;
        }

        public static void LoadMap()
        {
            LoadMap(Settings.active.mapSettings.map, Settings.active.shaderType);
        }
        public static void LoadMap(Map map)
        {
            LoadMap(map, Settings.active.shaderType);
        }
        public static void LoadMap(ShaderType shaderType)
        {
            LoadMap(Settings.active.mapSettings.map, shaderType);   
        }
        public static void LoadMap(Map map, ShaderType shaderType)
        {
            Debug.Log("Load map");
            
            if (Instance.currentMap != null)
                Instance.currentMap.Unload();

            
            Settings.active.mapSettings.map = map;
            Settings.active.shaderType = shaderType;
            
            switch(Settings.active.shaderType)
            {
                case ShaderType.HLSL:   
                    Instance.HLSLMaps[Settings.active.mapSettings.map].Load();
                    Instance.currentMap = Instance.HLSLMaps[Settings.active.mapSettings.map];
                    break;
                case ShaderType.ShaderGraph:
                    Instance.SGMaps[Settings.active.mapSettings.map].Load();
                    Instance.currentMap = Instance.SGMaps[Settings.active.mapSettings.map];
                    break;
            }
        }
    }
}
