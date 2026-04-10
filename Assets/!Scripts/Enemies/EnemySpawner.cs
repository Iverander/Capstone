using System.Threading.Tasks;
using UnityEngine;

namespace Capstone
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Enemy normalGoon;
        [SerializeField] Enemy bigGoon;
        [SerializeField] int amountToSpawn = 2;
        int bigGoonsToSpawn = 1;

        int spawnPlace;
        bool oneSpawn = false;
        Transform currentSpawn;

        [SerializeField] int spawnCalc;

        void Start()
        {
            RoundManager.onNewRound.AddListener(SpawnEnemies);
        }

        //called from RoundManager
        void SpawnEnemies()
        {
            oneSpawn = SpawnManager.instance == null;

            SpawnGoons();
            SpawnBigGoon();
        }

        async void SpawnGoons()
        {
            //spawns amountToSpawn at spawnpoints, to add more spawn points add more under spawnPoints (unity), with a delay
            for (int i = 0; i < RoundManager.round*2; i++)
            {
                Debug.Log(CalculateSpawnPoint());
                Instantiate(normalGoon.gameObject, CalculateSpawnPoint(), Quaternion.identity, transform);

                await Task.Delay(1000);
            }
            //CalculateAmount(ref amountToSpawn);
        }
        async void SpawnBigGoon()
        {
            //spawns amountToSpawn at spawnpoints, to add more spawn points add more under spawnPoints (unity), with a delay
            for (int i = 0; i < RoundManager.round/5; i++)
            {
                Instantiate(bigGoon.gameObject, CalculateSpawnPoint(), Quaternion.identity, transform);

                await Task.Delay(1000);
            }
            //CalculateAmount(ref bigGoonsToSpawn);
        }

        //void CalculateAmount(ref int amount)
        //{
        //    if (spawnCalc == 1)
        //    {
        //        amount *= 2;
        //    }
        //    else { amount++; }
        //}

        Vector3 CalculateSpawnPoint()
        {
            spawnPlace = Random.Range(0, SpawnManager.instance.spawnPoints.Count);
            return SpawnManager.instance.spawnPoints[spawnPlace].position;
        }
    }
}