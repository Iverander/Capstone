using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Firebase.Database;

namespace Capstone
{
    public class DataManager : MonoBehaviour
    {
        public static Data data;
        public string usrID;
        public UnityEvent<string> DataUpdated;
        
        DatabaseReference database;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            database = FirebaseDatabase.DefaultInstance.RootReference;
            
            UpdateData();
        }

        public void UpdateData()
        {
            data = new Data(System.Environment.UserName, SystemInfo.operatingSystem, SystemInfo.processorModel, SystemInfo.graphicsDeviceName, SystemInfo.systemMemorySize);
            DataUpdated.Invoke(data.ToString());
            Save();
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(data);
            database.Child("users").Child(data.name).SetRawJsonValueAsync(json);
        }
    }
}
