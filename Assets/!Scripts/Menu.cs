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
            DataManager.NewSection("Exit to Main Menu");
            SceneManager.LoadScene(1);
        }
        
    }
}
