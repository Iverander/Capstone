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
    public class Scene
    {
        [Scene] public string name;
        public LoadSceneMode loadSceneMode;
        public AsyncOperation operation { get; private set; }
        public float LoadingProgress => operation.progress * 100;

        public IEnumerator Load(bool autoActivate = true)
        {
            operation = SceneManager.LoadSceneAsync(name, loadSceneMode);
            operation.allowSceneActivation = autoActivate;

            while(operation.progress < .9f && Application.isPlaying)
            {
                Debug.Log(name + ": " + LoadingProgress);
                yield return null;
            }
        }

        public void Activate()
        {
            if(operation == null) throw new Exception("Cannot activate unloaded scene");
            if(operation.progress <.9f) throw new Exception("Scene not ready for activation");

            operation.allowSceneActivation = true;
        }
        public void Deactivate()
        {
            SceneManager.UnloadSceneAsync(name);
        }
    }
}
