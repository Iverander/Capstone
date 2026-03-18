using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

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

                public Section(string name, int averageFramerate,  int round)
                {
                    this._name = name;
                    this.averageFramerate = averageFramerate;
                    this.round = round;
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
        
        public List<Session> Sessions = new();
        
        public UnityEvent<string> DataUpdated;
        
        string json => JsonUtility.ToJson(this);

        public void Initialize()
        {
            OS = SystemInfo.operatingSystem;
            CPU = SystemInfo.processorType;
            GPU = SystemInfo.graphicsDeviceName;
            RAM = SystemInfo.systemMemorySize;

            DataManager.database.Child("users").Child(Environment.UserName).SetRawJsonValueAsync(json);
            
            StartNewSession("Initialization");
            UpdateData();
        }

        public void StartNewSession(string sessionName)
        {
            Sessions.Add(new Session(sessionName));
        }
        public void NewSection(string sectionName)
        {
            if (Sessions.Count <= 0)
            {
                Debug.Log("Create a session first!");
                return;
            }
            Sessions[^1].NewSection(sectionName);
        }
        
        public void Save()
        {
            #if !UNITY_EDITOR //only update firebase if it's a build
            DataManager.database.Child("users").Child(Environment.UserName).SetRawJsonValueAsync(json);
            #endif
            UpdateData();
        }
        
        public void UpdateData()
        {
            DataUpdated.Invoke(json);
        }
    }
}
