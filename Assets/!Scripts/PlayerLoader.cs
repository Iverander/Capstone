using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
    public class PlayerLoader : MonoBehaviour
    {
        [Scene] [SerializeField] private string playerScene;

        private void Start()
        {
            SceneManager.LoadScene(playerScene, LoadSceneMode.Additive);
        }
    }
}