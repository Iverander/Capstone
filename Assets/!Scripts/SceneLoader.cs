using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
    public class SceneLoader : MonoBehaviour
    {
        [Serializable]
        public class Scene
        {
            [Scene] public string name;
            public bool loaded;
            public bool activateOnLoad;

            public Scene(string name,  bool loaded = false, bool activateOnLoad = false)
            {
                this.name = name;
                SceneManager.LoadScene(name, LoadSceneMode.Single);
            }
        }
        
        public static SceneLoader Instance { get; private set; }
        [SerializedDictionary] public List<Scene> preLoadScenes = new();
        public static Dictionary<string, Scene> LoadedScenes = new();

        private void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var scene in preLoadScenes)
            {
                LoadScene(scene.name);
            }
        }

        public async void AsyncLoadScene(string scene)
        {
            LoadedScenes.Add(scene, new Scene(scene));
        }
        
        public static void LoadScene(string sceneName)
        {
            if(LoadedScenes.ContainsKey(sceneName)) 
                LoadedScenes[sceneName].loaded = true;
            else
                Instance.AsyncLoadScene(sceneName);
        }
    }
}
