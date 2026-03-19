using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Capstone
{
    [Serializable]
    public class Data
    {
        [Serializable]
        public class Session
        {
            public string _name;
            public string dateTime;
            public List<Section> sections = new();
            public string mapSettings;

            private float timeStart;
            private float frameStart;
                
            [Serializable]
            public struct Section
            {
                public string _name;
                public int averageFramerate;
                public int round;
                public float cpuPercentage;
                public float gpuPercentage;
                public int usedRam;

                public Section(string name, int averageFramerate,  int round)
                {
                    this._name = name;
                    this.averageFramerate = averageFramerate;
                    this.round = round;

                    cpuPercentage = -1;
                    gpuPercentage = -1;
                    usedRam = -1;
                    
                    if (DataManager.CPUPercentage > 0 && DataManager.CPUPercentage < 100)
                        cpuPercentage = DataManager.CPUPercentage;
                }
            }

            public Session(string sessionName)
            {
                _name = sessionName;
                this.dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.mapSettings = LevelSettings.CurrentMapSettings.ToString();
                this.timeStart = Time.time;
                this.frameStart = Time.frameCount;
                
                NewSection($"--{sessionName} start--");
            }

            public void NewSection(string sectionName)
            {
                sections.Add(new Section(
                    name: sectionName,
                    averageFramerate: Time.frameCount > 0 ? Mathf.RoundToInt((Time.frameCount - frameStart) / (Time.time - timeStart)) : -1,
                    round: RoundManager.instance != null ? RoundManager.round : -1
                    )
                    
                );
            }
        }
        
        
        public string OS;
        public string CPU;
        public string GPU;
        public int RAM;
        public string resolution;
        
        public List<Session> sessions = new();
        
        public UnityEvent<string> DataUpdated;
        
        string json => JsonUtility.ToJson(this);

        public void Initialize()
        {
            OS = SystemInfo.operatingSystem;
            CPU = SystemInfo.processorType;
            GPU = SystemInfo.graphicsDeviceName;
            RAM = SystemInfo.systemMemorySize;
            resolution = $"{Screen.width}x{Screen.height}";

            //DataManager.database.Child("users").Child(Environment.UserName).SetRawJsonValueAsync(json);
            
            StartNewSession("Initialization");
            UpdateData();
        }

        public void StartNewSession(string sessionName)
        {
            sessions.Add(new Session(sessionName));
        }
        public void NewSection(string sectionName)
        {
            if (sessions.Count <= 0)
            {
                Debug.Log("Create a session first!");
                return;
            }
            sessions[^1].NewSection(sectionName);
        }
        
        public void Save()
        {
            #if UNITY_EDITOR //only update firebase if it's a build
            DataManager.database.Child("devUsers").Child(SystemInfo.deviceUniqueIdentifier).SetRawJsonValueAsync(json);
            #elif !UNITY_EDITOR
            DataManager.database.Child("users").Child(SystemInfo.deviceUniqueIdentifier).SetRawJsonValueAsync(json);
            #endif
            UpdateData();
        }
        
        public void UpdateData()
        {
            DataUpdated.Invoke(json);
        }
    }
}
