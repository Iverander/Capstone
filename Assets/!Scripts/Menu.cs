using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Capstone
{
    public class Menu : MonoBehaviour
    {
        void Start()
        {
            Player.input.onMenu.AddListener(OpenMenu);
        }

        private void OpenMenu()
        {
            SceneManager.LoadScene(0);
        }
        
    }
}
