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

namespace Capstone
{
    public class DataManager : MonoBehaviour
    {
        public Data data = new Data();
        
        public static DatabaseReference database;
        
        public static DataManager instance;
        public static float CPUPercentage;

        private Thread cpuThread;
    
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
            
            data.Initialize();
            
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
                Debug.Log("GPU%: " + new PerformanceCounter("GPU Engine", "Utilization Percentage"));
        }

        private void OnDestroy()
        {
           cpuThread.Abort(); 
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
                CPUPercentage = 100 * (float)cpuDiff.TotalSeconds / timeout / ProcessorCount;
            
                //Debug.Log(CPUPercentage);
                Thread.Sleep((int)(timeout * 1000));
            }
        }

        private void OnApplicationQuit()
        {
            //data.StartNewSession("Application Quit");
            data.Save();
        }

        public static void StartNewSession(string sessionName)
        {
            if (instance == null) return;
            
            Debug.Log("Starting new session " + sessionName);
            instance.data.StartNewSession(sessionName);
        }
        public static void NewSection(string sectionName)
        {
            if (instance == null) return;
            
            Debug.Log("Starting new section " + sectionName);
            instance.data.NewSection(sectionName);
        }
    }
}
