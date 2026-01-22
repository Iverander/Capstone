using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Firebase.Database;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine.Events;

namespace Capstone
{
    [Serializable]
    public class Data
    {
        [Serializable]
        public class Section
        {
            public int averageFramerate;
            
            public Section()
            {
                if (Time.frameCount == 0)
                {
                    averageFramerate = -1;
                    return;
                }
                averageFramerate = Mathf.RoundToInt(Time.frameCount / Time.time);
            }
        }
        public string OS;
        public string CPU;
        public string GPU;
        public int RAM;
        
        public SerializedDictionary<string, Section> Sections = new();
        
        //public List<float> framerate = new List<float>();
        [JsonIgnore]
        public UnityEvent<string> DataUpdated;
        
        string json => JsonUtility.ToJson(this);

        public void Initialize()
        {
            Sections.Add("Initilization", new Section());
            
            OS = SystemInfo.operatingSystem;
            CPU = SystemInfo.processorType;
            GPU = SystemInfo.graphicsDeviceName;
            RAM = SystemInfo.systemMemorySize;

            DataManager.database.Child("users").Child(Environment.UserName).SetRawJsonValueAsync(json);
            UpdateData();
        }

        public void NewSection(string sectionName)
        {
            Sections.Add(sectionName, new Section());
        }
        
        public void Save()
        {
            DataManager.database.Child("users").Child(Environment.UserName).SetRawJsonValueAsync(json);
            UpdateData();
        }
        
        public void UpdateData()
        {
            DataUpdated.Invoke(json);
        }
    }
}
