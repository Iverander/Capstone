using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Capstone
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager instance;

        [ReadOnly] public List<Transform> spawnPoints = new();

        void Start()
        {
            instance = this;

            foreach (Transform child in transform)
            {
                spawnPoints.Add(child);
            }
        }
    }
}
