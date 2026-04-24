using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

namespace Capstone
{
    [Serializable]
    public class Data
    {
        [Serializable]
        public struct Section
        {
            public string _name;
            public string _date;
            public string _time;
            
            public int averageFramerate;
            public int round;
            public float cpuPercentage;
            public float usedVramMB;
            public float usedRamMB;

            public Section(string name, int averageFramerate,  int round)
            {
                this._name = name;
                this._date = DateTime.Today.Date.ToString("MM/dd/yyyy");
                this._time = DateTime.Now.ToString("hh:mm:ss");
                
                this.averageFramerate = averageFramerate;
                this.round = round;

                this.cpuPercentage = -1;

                usedVramMB = Profiler.GetAllocatedMemoryForGraphicsDriver() / 1048576;
                usedRamMB = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
                
                if (DataManager.CPUPercentage > 0 && DataManager.CPUPercentage < 100)
                    cpuPercentage = DataManager.CPUPercentage;
            }
        }
        [Serializable]
        public class Session
        {
            
            public string _name;
            public List<Section> sections = new();
            public Settings levelSettings;

            private float timeStart;
            private float frameStart;
                

            public Session(string sessionName)
            {
                _name = sessionName;
                this.levelSettings = Settings.active;
                this.timeStart = Time.time;
                this.frameStart = Time.frameCount;
                
                //NewSection($"--{sessionName} start--");
            }

            public void NewSection(string sectionName)
            {
                sections.Add(new Section(
                    name: sectionName,
                    averageFramerate: Time.frameCount > 0 ? Mathf.RoundToInt((Time.frameCount - frameStart) / (Time.time - timeStart)) : -1,
                    round: RoundManager.instance != null ? RoundManager.round : -1
                    )
                );

                timeStart = Time.time;
            }
        }
        
        
        public string OS;
        public string CPU;
        public string GPU;
        public int RamMB;
        public int VramMB;
        public string resolution;
        public bool Developer;
        
        public List<Session> sessions = new();
        
        string json => JsonUtility.ToJson(this);

        public void Initialize()
        {
            OS = SystemInfo.operatingSystem;
            CPU = SystemInfo.processorType;
            GPU = SystemInfo.graphicsDeviceName;
            RamMB = SystemInfo.systemMemorySize;
            VramMB = SystemInfo.graphicsMemorySize;
            resolution = $"{Screen.width}x{Screen.height}";
            
#if UNITY_EDITOR
            Developer = true;
#else 
            Developer = false;
#endif

            //DataManager.database.Child("users").Child(Environment.UserName).SetRawJsonValueAsync(json);
            
            //StartNewSession("Initialization");
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
            DataManager.database.Child(SystemInfo.deviceUniqueIdentifier).SetRawJsonValueAsync(json);

        }
    }
}
