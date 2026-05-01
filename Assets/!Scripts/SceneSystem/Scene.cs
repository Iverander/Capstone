using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSystem
{
    public abstract class SceneObject : ScriptableObject
    {
        public abstract bool isActive { get; }
        public AsyncOperation operation { get; protected set; }
        public abstract void Load(bool autoActivate = true);
        public abstract void Unload();
    } 
    
    [CreateAssetMenu(fileName = "Scene", menuName = "Scriptable Objects/SceneSystem/Scene")]
    [Serializable]
    public class Scene : SceneObject
    {
        [Scene] public string scene;
        public LoadSceneMode loadSceneMode;
        public float LoadingProgress => operation.progress * 100;


        public override bool isActive => false;

        public override void Load(bool autoActivate = true)
        {
            operation = SceneManager.LoadSceneAsync(scene, loadSceneMode);
            operation.allowSceneActivation = autoActivate;
        }

        public override void Unload()
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
