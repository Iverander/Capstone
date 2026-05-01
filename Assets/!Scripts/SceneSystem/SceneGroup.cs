using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSystem
{
    [CreateAssetMenu(fileName = "SceneGroup", menuName = "Scriptable Objects/SceneSystem/SceneGroup")]
    [Serializable]
    public class SceneGroup : SceneObject
    {
        [Scene] public List<string> scenes;
        public LoadSceneMode loadSceneMode;

        public override bool isActive => false;

        public override void Load(bool autoActivate = true)
        {
            operation = SceneManager.LoadSceneAsync(scenes[0], loadSceneMode);
            operation.allowSceneActivation = autoActivate;
            
            for (int i = 1; i < scenes.Count; i++)
            {
                SceneManager.LoadSceneAsync(scenes[i], LoadSceneMode.Additive);
            }
        }
        
        public override void Unload()
        {
            foreach (var scene in scenes)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
            
        }
    }
}
