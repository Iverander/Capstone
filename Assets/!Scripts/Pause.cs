using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Capstone
{
    public class Pause : MonoBehaviour
    {
        void Start()
        {
            Player.input.onMenu.AddListener(OpenMenu);
        }

        private void OpenMenu()
        { 
            MenuManager.OpenMenu(MenuManager.Menu.Pause);
        }
        
    }
}
