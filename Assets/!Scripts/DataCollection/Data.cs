using System;
using System.Collections.Generic;
using Google.MiniJSON;
using UnityEngine;

namespace Capstone.Datapoints
{
    /// <summary>
    ///     Low1%, average and High1%
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
        public float _durationS;
        public float _afkDurationS;
        public int _round;
        //public Settings _levelSettings;
        public string _shader;
        
        public LowsAvgHighs<float> fps;

        //public LowsAvgHighs<float> frameTimingS;
        public LowsAvgHighs<double> gpuFrameTimingMS;

        //public LowsAvgHighs<float> batches;
        //public LowsAvgHighs<float> usedVramMB;
        public LowsAvgHighs<float> usedRamMB;

        public int highestEnemyCount;


        public RoundData(float duration)
        {
            highestEnemyCount = RoundManager.highestEnemyCount;

            fps = new LowsAvgHighs<float>(
                Mathf.RoundToInt(Arithmetic.Lows(DataManager.fpsValues)),
                Mathf.RoundToInt(Arithmetic.Average(DataManager.fpsValues)),
                Mathf.RoundToInt(Arithmetic.Highs(DataManager.fpsValues))
            );
            gpuFrameTimingMS = new LowsAvgHighs<double>(
                Arithmetic.Lows(DataManager.gpuFrameTimings),
                Arithmetic.Average(DataManager.gpuFrameTimings),
                Arithmetic.Highs(DataManager.gpuFrameTimings)
            );
            /*
            this.frameTimingS = new(
                lows: Arithmetic.Lows(DataManager.frameTimings),
                average: Arithmetic.Average(DataManager.frameTimings),
                highs: Arithmetic.Highs(DataManager.frameTimings)
            );
            this.batches = new(
                lows: Arithmetic.Lows(DataManager.batches),
                average: Arithmetic.Average(DataManager.batches),
                highs: Arithmetic.Highs(DataManager.batches)
                );*/
            /*usedVramMB = new LowsAvgHighs<float>(
                Arithmetic.Lows(DataManager.usedVRam),
                Arithmetic.Average(DataManager.usedVRam),
                Arithmetic.Highs(DataManager.usedVRam)
            );*/
            
            Debug.Log("GPU Lows: " + Arithmetic.Lows(DataManager.gpuFrameTimings));
            usedRamMB = new LowsAvgHighs<float>(
                Arithmetic.Lows(DataManager.usedRam),
                Arithmetic.Average(DataManager.usedRam),
                Arithmetic.Highs(DataManager.usedRam)
            );

            /*
            this.cpuTime = new(
                lows: DataManager.Lows(DataManager.cpuTimes),
                average: DataManager.Average(DataManager.cpuTimes),
                highs:-1);*/

            //_levelSettings = Settings.active;
            _shader = Settings.active.shaderType.ToString();
            
            _durationS = duration;
            _afkDurationS = Player.instance.afkTime;
            _round = RoundManager.round;
        }
    }

    [Serializable]
    public class Session
    {
        public static Session active;
        public List<RoundData> rounds = new();

        private string _name;
        
        public string _weather;
        public string _map;
        public string _obstacles;
        
        private float sectionStart;


        public Session(string sessionName)
        {
            _name = sessionName;
            DataManager.collectData = true;
            DataManager.ResetData();
            sectionStart = Time.time;
            active = this;
            
            _map = Settings.active.mapSettings.map.ToString();
            _obstacles = Settings.active.mapSettings.obstacles.ToString();
            _weather = Settings.active.mapSettings.weatherType.ToString();
        }

        private string json => JsonUtility.ToJson(this);

        public void NewSection()
        {
            rounds.Add(new RoundData(Mathf.RoundToInt(Time.time - sectionStart)));
            sectionStart = Time.time;
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
        //public string graphicsAPI;
        public bool developer;

        private string json => JsonUtility.ToJson(this);

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
            developer = false;
#endif
            Save();
        }

        public void Save()
        {
            DataManager.database.Child(SystemInfo.deviceUniqueIdentifier).Child("Hardware").SetRawJsonValueAsync(json);
        }
    }
}