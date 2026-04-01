using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
    [Serializable]
    public class SceneGroup
    {
        public Scene[] scenes;

        public void Load()
        {
            foreach (var scene in scenes)
            {
                scene.Load();   
            }
        }
    }
    [Serializable]
    public class Scene
    {
        [Scene] public string name;
        public LoadSceneMode loadSceneMode;
        public AsyncOperation operation { get; private set; }
        public float LoadingProgress => operation.progress * 100;


        public void Load(bool autoActivate = true)
        {
            operation = SceneManager.LoadSceneAsync(name, loadSceneMode);
            operation.allowSceneActivation = autoActivate;
        }

        public void Unload()
        {
            SceneManager.UnloadSceneAsync(name);
        }
    }
}
