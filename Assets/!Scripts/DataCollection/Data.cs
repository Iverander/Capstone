using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
namespace Capstone
{
    [Serializable]
    public class Data
    {
        public string name;
        public string OS;
        public string CPU;
        public string GPU;
        public int RAM;
        
        public float averageFramerate;
        //public List<float> framerate = new List<float>();

        public void Initialize()
        {
            name = Environment.UserName;
            OS = SystemInfo.operatingSystem;
            CPU = SystemInfo.processorType;
            GPU = SystemInfo.graphicsDeviceName;
            RAM = SystemInfo.systemMemorySize;
            
            averageFramerate = -1f;
            
            Save();
        }

        public override string ToString()
        {
            return $"{OS} \n\n{CPU} \n\n{GPU} \n\n{RAM}";
        }
        
        public void Save()
        {
            averageFramerate = Time.frameCount / Time.time;
            
            string json = JsonUtility.ToJson(this);

            DataManager.database.Child("users").Child(name).SetRawJsonValueAsync(json);
        }

        public void UpdateFramerate()
        {
            
            //framerate.Add(1f / Time.deltaTime);
        }
    }
}
