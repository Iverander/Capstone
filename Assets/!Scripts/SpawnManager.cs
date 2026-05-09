using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager instance;

        [ReadOnly] public List<Transform> spawnPoints = new();

        private void Start()
        {
            instance = this;

            foreach (Transform child in transform) spawnPoints.Add(child);
        }
    }
}