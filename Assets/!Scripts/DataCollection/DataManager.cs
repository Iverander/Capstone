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
        public static Data data = new Data();
        public UnityEvent<string> DataUpdated;
        
        public static DatabaseReference database;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            database = FirebaseDatabase.DefaultInstance.RootReference;
            
            data.Initialize();
        }

        private void OnApplicationQuit()
        {
            data.Save();
        }

        private void Update()
        {
            data.UpdateFramerate();
        }

        public void UpdateData()
        {
            DataUpdated.Invoke(data.ToString());
        }
    }
}
