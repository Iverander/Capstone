using System.Threading.Tasks;
using UnityEngine;

namespace Capstone
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy normalGoon;
        [SerializeField] private Enemy bigGoon;
        [SerializeField] private Enemy quickGoon;
        //[SerializeField] private int amountToSpawn = 2;

        [SerializeField] private int spawnCalc;
       // private int bigGoonsToSpawn = 1;
        private Transform currentSpawn;
        private bool oneSpawn;

        private int spawnPlace;

        private void Start()
        {
            RoundManager.onNewRound.AddListener(SpawnEnemies);
        }

        //called from RoundManager
        private void SpawnEnemies()
        {
            oneSpawn = SpawnManager.instance == null;

            SpawnGoons();
            SpawnQuickGoons();
            SpawnBigGoon();
        }

        private async void SpawnGoons()
        {
            //spawns amountToSpawn at spawnpoints, to add more spawn points add more under spawnPoints (unity), with a delay
            for (var i = 0; i < 1 + (RoundManager.round - 1) * 3; i++)
            {
                Debug.Log(CalculateSpawnPoint());
                Instantiate(normalGoon.gameObject, CalculateSpawnPoint(), Quaternion.identity, transform);

                await Task.Delay(1000);
            }
            //CalculateAmount(ref amountToSpawn);
        }

        private async void SpawnQuickGoons()
        {
            //spawns amountToSpawn at spawnpoints, to add more spawn points add more under spawnPoints (unity), with a delay
            for (var i = 0; i < RoundManager.round / 3 * 4; i++)
            {
                Debug.Log(CalculateSpawnPoint());
                Instantiate(quickGoon.gameObject, CalculateSpawnPoint(), Quaternion.identity, transform);

                await Task.Delay(1000);
            }
            //CalculateAmount(ref amountToSpawn);
        }

        private async void SpawnBigGoon()
        {
            //spawns amountToSpawn at spawnpoints, to add more spawn points add more under spawnPoints (unity), with a delay
            for (var i = 0; i < RoundManager.round / 7 * 2; i++)
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

        private Vector3 CalculateSpawnPoint()
        {
            spawnPlace = Random.Range(0, SpawnManager.instance.spawnPoints.Count);
            return SpawnManager.instance.spawnPoints[spawnPlace].position;
        }
    }
}