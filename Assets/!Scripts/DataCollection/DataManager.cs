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
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            database = FirebaseDatabase.DefaultInstance.RootReference;
            
            data.Initialize();
        }

        private void OnApplicationQuit()
        {
            data.NewSection("Quit");
            data.Save();
        }
    }
}
