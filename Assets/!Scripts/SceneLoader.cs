using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
    public class SceneLoader : MonoBehaviour
    {
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
