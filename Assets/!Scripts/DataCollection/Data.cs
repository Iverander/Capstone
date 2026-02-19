using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone
{
    [Serializable]
    public class Data
    {
        [Serializable]
        public struct Section
        {
            public int averageFramerate;
        }
        public string OS;
        public string CPU;
        public string GPU;
        public int RAM;
        
        public SerializedDictionary<string, Section> Sections = new();
        
        public UnityEvent<string> DataUpdated;
        
        string json => JsonUtility.ToJson(this);

        public void Initialize()
        {
            OS = SystemInfo.operatingSystem;
            CPU = SystemInfo.processorType;
            GPU = SystemInfo.graphicsDeviceName;
            RAM = SystemInfo.systemMemorySize;

            DataManager.database.Child("users").Child(Environment.UserName).SetRawJsonValueAsync(json);
            
            NewSection("Initilization");
            UpdateData();
        }

        public void NewSection(string sectionName)
        {
            Section section = new Section();
            
            if (Time.frameCount == 0)
            {
                section.averageFramerate = -1;
                return;
            }
            section.averageFramerate = Mathf.RoundToInt(Time.frameCount / Time.time);
            
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
