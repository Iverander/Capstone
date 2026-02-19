using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Firebase.Database;

namespace Capstone
{
    public class DataManager : MonoBehaviour
    {
        public Data data = new Data();
        
        public static DatabaseReference database;
        
        public static DataManager instance;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            instance = this;
            database = FirebaseDatabase.DefaultInstance.RootReference;
            
            data.Initialize();
            
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            data.NewSection("Quit");
            data.Save();
        }
    }
}
