using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

namespace Capstone.Datapoints
{
    /// <summary>
    /// Low1%, average and High1%
    /// </summary>
    /// <typeparam name="T">accuracy</typeparam>
    [Serializable]
    public struct LowsAvgHighs<T>
    {
        public T Lows;
        public T Average;
        public T Highs;
        
        public LowsAvgHighs(T lows, T average, T highs)
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
        public float _durationS;
        public float _afkDurationS;
        public int _round;
        public Settings _levelSettings;
        
        public LowsAvgHighs<float> fps;
        public LowsAvgHighs<float> frameTimingS;
        public LowsAvgHighs<double> gpuFrameTimingMS;
        public LowsAvgHighs<float> batches;
        public LowsAvgHighs<float> usedVramMB; 
        public LowsAvgHighs<float> usedRamMB;
        
        public int highestEnemyCount;
        


        public RoundData(string context, float duration)
        {
            this._context = context;

            this.highestEnemyCount = RoundManager.highestEnemyCount;
            
            this.fps = new(
                lows: Arithmetic.Lows(DataManager.fpsValues), 
                average: Arithmetic.Average(DataManager.fpsValues), 
                highs: Arithmetic.Highs(DataManager.fpsValues)
                );
            this.gpuFrameTimingMS = new(
                lows: Arithmetic.Lows(DataManager.gpuFrameTimings), 
                average: Arithmetic.Average(DataManager.gpuFrameTimings),
                highs: Arithmetic.Highs(DataManager.gpuFrameTimings)
                );
            this.frameTimingS = new(
                lows: Arithmetic.Lows(DataManager.frameTimings), 
                average: Arithmetic.Average(DataManager.frameTimings),
                highs: Arithmetic.Highs(DataManager.frameTimings)
            );
            this.batches = new(
                lows: Arithmetic.Lows(DataManager.batches), 
                average: Arithmetic.Average(DataManager.batches),
                highs: Arithmetic.Highs(DataManager.batches)
                );
            this.usedVramMB = new(
                lows: Arithmetic.Lows(DataManager.usedVRam),
                average: Arithmetic.Average(DataManager.usedVRam),
                highs: Arithmetic.Highs(DataManager.usedVRam)
            );
            this.usedRamMB = new(
                lows: Arithmetic.Lows(DataManager.usedRam),
                average: Arithmetic.Average(DataManager.usedRam),
                highs: Arithmetic.Highs(DataManager.usedRam)
            );            
            
            /*
            this.cpuTime = new(
                lows: DataManager.Lows(DataManager.cpuTimes), 
                average: DataManager.Average(DataManager.cpuTimes),
                highs:-1);*/
            
            this._levelSettings = Settings.active;
            this._durationS = duration;
            this._round = RoundManager.round;
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
        public int ramMB;
        public int vramMB;
        public string resolution;
        public string graphicsAPI;
        public bool developer;
        
        string json => JsonUtility.ToJson(this);

        public void Initialize()
        {
            OS = SystemInfo.operatingSystem;
            CPU = SystemInfo.processorType;
            GPU = SystemInfo.graphicsDeviceName;
            ramMB = SystemInfo.systemMemorySize;
            vramMB = SystemInfo.graphicsMemorySize;
            resolution = $"{Screen.width}x{Screen.height}";
            
            //if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
            //    graphicsAPI = PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64);
            
#if UNITY_EDITOR
            developer = true;
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
