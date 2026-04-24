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

namespace Capstone.Datapoints
{
    [Serializable]
    public struct LowsAvgHighs
    {
        public float Lows;
        public float Average;
        public float Highs;
        
        public LowsAvgHighs(float lows, float average, float highs)
        {
            Lows = lows;
            Average = average;
            Highs = highs;
        }
    }
    
    [Serializable]
    public class RoundData
    {
        public string _context;
        public float _durationSeconds;
        public int _round;
        public Settings _levelSettings;
        
        public LowsAvgHighs fps;
        public LowsAvgHighs renderTime;
        public LowsAvgHighs batches;
        public LowsAvgHighs cpuTime;
        
        public int highestEnemyCount;
        
        public float usedVramMB;
        public float usedRamMB;

        public RoundData(string context, float duration)
        {
            this._context = context;

            this.highestEnemyCount = RoundManager.highestEnemyCount;
            
            this.fps = new(
                lows: DataManager.Lows(DataManager.fpsValues), 
                average: DataManager.Average(DataManager.fpsValues), 
                highs: DataManager.Highs(DataManager.fpsValues));
            this.renderTime = new(
                lows: DataManager.Lows(DataManager.renderTimes), 
                average: DataManager.Average(DataManager.renderTimes),
                highs: DataManager.Highs(DataManager.renderTimes));
            this.batches = new(
                lows: DataManager.Lows(DataManager.batches), 
                average: DataManager.Average(DataManager.batches),
                highs: DataManager.Highs(DataManager.batches));
            /*
            this.cpuTime = new(
                lows: DataManager.Lows(DataManager.cpuTimes), 
                average: DataManager.Average(DataManager.cpuTimes),
                highs:-1);*/
            
            this._levelSettings = Settings.active;

            this._durationSeconds = duration;
            
            this._round = RoundManager.round;

            usedVramMB = Profiler.GetAllocatedMemoryForGraphicsDriver() / 1048576;
            usedRamMB = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
        }
    }
    [Serializable]
    public class Session
    {
        public static Session active;
        
        string _name;
        public List<RoundData> rounds = new();
        private float sectionStart;
        
        string json => JsonUtility.ToJson(this);
            

        public Session(string sessionName)
        {
            _name = sessionName;
            DataManager.collectData = true;
            DataManager.ResetData();
            this.sectionStart = Time.time;
            active = this;
        }

        public void NewSection(string context)
        {
            rounds.Add(new RoundData(context, Mathf.RoundToInt(Time.time - sectionStart)));
            this.sectionStart = Time.time;
            DataManager.ResetData();
        }
        
        public void Save()
        {
            DataManager.database.Child(SystemInfo.deviceUniqueIdentifier).Child(_name).SetRawJsonValueAsync(json);
        }
    }
   
    [Serializable]
    public class HardwareData
    {
        public string OS;
        public string CPU;
        public string GPU;
        public int RamMB;
        public int VramMB;
        public string resolution;
        public bool Developer;
        
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
            Save();
        }
        
        public void Save()
        {
            DataManager.database.Child(SystemInfo.deviceUniqueIdentifier).Child("Hardware").SetRawJsonValueAsync(json);
        }
    }
}
