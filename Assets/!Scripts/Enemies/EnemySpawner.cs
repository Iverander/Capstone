using System.Collections;
using UnityEngine;

namespace Capstone
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Enemy enemy;
        int amountToSpawn = 2;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(Spawning());
        }
        
        IEnumerator Spawning()
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                yield return new WaitForSeconds(1);
                Instantiate(enemy.gameObject, transform.position, transform.rotation);
            }
        }
    }
}
