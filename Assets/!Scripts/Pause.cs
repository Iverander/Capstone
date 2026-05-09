using UnityEngine;

namespace Capstone
{
    public class Pause : MonoBehaviour
    {
        private void Start()
        {
            Player.input.onMenu.AddListener(OpenMenu);
        }

        private void OpenMenu()
        {
            MenuManager.OpenMenu(MenuManager.Menu.Pause);
        }
    }
}