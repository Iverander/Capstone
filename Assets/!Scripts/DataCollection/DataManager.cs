using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Firebase.Database;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;
using UnityEditor;
using Capstone.Datapoints;
using Unity.Profiling;

namespace Capstone
{
    public class DataManager : MonoBehaviour
    {
        public HardwareData hardwareData = new HardwareData();
        public List<Session> sessions = new List<Session>();
        
        
        public static DatabaseReference database;
        
        public static DataManager instance;
        
        private FrameTiming[] captureData;

        public static bool collectData;
        public static List<float> fpsValues = new();
        public static List<float> batches = new();
        public static List<double> gpuFrameTimings = new();
        public static List<float> frameTimings = new();
        public static List<float> usedVRam = new();
        public static List<float> usedRam = new();
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            instance = this;
            database = FirebaseDatabase.DefaultInstance.RootReference;
            
            hardwareData.Initialize();
            
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (collectData)
            {
                fpsValues.Add(Mathf.RoundToInt(1f / Time.deltaTime));
                batches.Add(UnityStats.batches);
                frameTimings.Add(UnityStats.frameTime);
                usedVRam.Add(Profiler.GetAllocatedMemoryForGraphicsDriver() / 1048576);
                usedRam.Add(Profiler.GetTotalAllocatedMemoryLong() / 1048576);
                
                FrameTimingManager.CaptureFrameTimings();
                captureData = new FrameTiming[1];//FrameTimingManager.GetGpuTimerFrequency();
                FrameTimingManager.GetLatestTimings(1, captureData);
                
                Debug.Log(captureData[0].gpuFrameTime);
                
                gpuFrameTimings.Add(captureData[0].gpuFrameTime);
            }

            //if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
            //    Debug.Log("GPU%: " + new PerformanceCounter("GPU Engine", "Utilization Percentage"));
        }

        public static void ResetData()
        {
            fpsValues.Clear();
        }

        private void OnApplicationQuit()
        {
            //data.StartNewSession("Application Quit");
            hardwareData.Save();

            foreach (var session in sessions)
            {
                session.Save();
            }
        }

        public void StartNewSession()
        {
            Debug.Log("Starting new session");
            sessions.Add(new Session($"Session {sessions.Count + 1}"));
        }
    }
}
