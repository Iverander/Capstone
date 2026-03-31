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
            Debug.Log("OpenMenu");
            MenuManager.OpenMenu(MenuManager.Menu.Pause);
            //DataManager.NewSection("Exit to Menu");
            //SceneManager.LoadScene(1);
        }
        
    }
}
