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
            data.NewSection("Application Quit");
            data.Save();
        }

        public static void StartNewSession(string sessionName)
        {
            if (instance == null) return;
            instance.data.StartNewSession(sessionName);
        }
        public static void NewSection(string sectionName)
        {
            if (instance == null) return;
            instance.data.NewSection(sectionName);
        }
    }
}
