using System.Collections;
using UnityEngine;

namespace Capstone
{
    public class EnemySpawner : MonoBehaviour
    {
        Enemy enemy;
        int amountToSpawn = 2;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            enemy = FindAnyObjectByType<Enemy>();
            StartCoroutine(Spawning());
        }
        
        IEnumerator Spawning()
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                yield return new WaitForSeconds(1);
                Instantiate(enemy);
            }
        }
    }
}
