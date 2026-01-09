using System;
using UnityEngine;
using Firebase.Database;
namespace Capstone
{
    [Serializable]
    public struct Data
    {
        public string name;
        public string OS;
        public string CPU;
        public string GPU;
        public int RAM;

        public override string ToString()
        {
            return $"{OS} \n\n{CPU} \n\n{GPU} \n\n{RAM}GB";
        }

        public Data(string name, string os ,string cpu, string gpu, int ram)
        {
            this.name = name;
            OS = os;
            CPU = cpu;
            GPU = gpu;
            RAM = ram / 1000;
        }
    }
}
