using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
    public class AutoChangeScene : MonoBehaviour
    {
        [Scene, SerializeField] private string scene;

        private void Start()
        {
            SceneManager.LoadScene(scene);
        }
    }
}
