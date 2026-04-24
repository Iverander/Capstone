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

namespace Capstone
{
    public class DataManager : MonoBehaviour
    {
        public HardwareData hardwareData = new HardwareData();
        public List<Session> sessions = new List<Session>();
        
        
        public static DatabaseReference database;
        
        public static DataManager instance;

        private Thread cpuThread;

        public static bool collectData;
        public static List<float> fpsValues = new();
        public static List<float> renderTimes = new();
        public static List<float> batches = new();
        public static List<float> cpuTimes = new();
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //Profiler.BeginThreadProfiling("main", "mainthread");
            
            Application.runInBackground = true;
            ProcessorCount = SystemInfo.processorCount/2;
            cpuThread = new Thread(RefreshCpuUsage)
            {
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            cpuThread.Start();
            
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
                renderTimes.Add(UnityStats.renderTime);
                batches.Add(UnityStats.batches);
            }

            //if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
            //    Debug.Log("GPU%: " + new PerformanceCounter("GPU Engine", "Utilization Percentage"));
        }

        private void OnDestroy()
        {
           cpuThread.Abort(); 
        }

        public static void ResetData()
        {
            fpsValues.Clear();
            renderTimes.Clear();
        }

        public static float Average(List<float>  values)
        {
            var average = 0f;

            foreach (var rate in values)
            {
                average += rate;
            }

            average /= values.Count;
            return average;
        }
        public static float Lows(List<float>  values)
        {
            var fpsLows = 0f; 
            values.Sort();

            for (int i = 0; i < Mathf.RoundToInt(values.Count / 100); i++)
            {
                fpsLows += values[i];
            }

            fpsLows /= Mathf.RoundToInt(values.Count / 100);
            return fpsLows;
        }
        public static float Highs(List<float>  values)
        {
            var fpsLows = 0f; 
            values.Sort();
            values.Reverse();

            for (int i = 0; i < Mathf.RoundToInt(values.Count / 100); i++)
            {
                fpsLows += values[i];
            }

            fpsLows /= Mathf.RoundToInt(values.Count / 100);
            return fpsLows;

        }
        
        private readonly float timeout = 1f;
        private TimeSpan lastCpuTime;
        private int ProcessorCount;
        void RefreshCpuUsage()
         {
            while (true)
            {
                Process[] processes = Process.GetProcesses();

                var cpuTime = TimeSpan.Zero;
                cpuTime = processes.Aggregate(cpuTime, (current, process) => current + process.TotalProcessorTime);
                var cpuDiff = cpuTime - lastCpuTime;
                lastCpuTime = cpuTime;
                cpuTimes.Add(100 * (float)cpuDiff.TotalSeconds / timeout / ProcessorCount);
            
                //Debug.Log(CPUPercentage);
                Thread.Sleep((int)(timeout * 1000));
            }
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
