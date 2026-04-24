using System;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using SceneSystem;
using UnityEngine;
//using UnityEngine.SceneManagement;

namespace Capstone
{
    public class MenuManager : MonoBehaviour
    {

        public enum Menu
        {
            None,
            Store,
            Pause
        }

        public static MenuManager instance;
        public SerializedDictionary<Menu, Scene> Menus = new();

        [field: SerializeField, ReadOnly] public Menu currentMenu {get; private set;}
        void Start()
        {
            instance = this;
            Player.instance.inputReader.onCloseMenu.AddListener(CloseMenu);
        }

        public static void OpenMenu(Menu menu)
        {
            if(instance.currentMenu != Menu.None) return;

            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            instance.Menus[menu].Load(true);
            instance.currentMenu = menu;
        } 
        public void CloseMenu()
        {
            if(instance.currentMenu == Menu.None) return;

            Debug.Log("Close!");
            Menus[currentMenu].Unload();
            currentMenu = Menu.None;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
}
