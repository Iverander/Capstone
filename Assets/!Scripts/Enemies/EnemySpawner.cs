using System.Threading.Tasks;
using UnityEngine;

namespace Capstone
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Enemy enemy;
        [SerializeField] int amountToSpawn = 2;

        [SerializeField] Transform spawnPoints;
        int spawnPlace;
        bool oneSpawn = false;
        Transform currentSpawn;

        void Start()
        {
            RoundManager.newRound.AddListener(SpawnEnemies);
            if (spawnPoints == null) oneSpawn = true;
            else oneSpawn = false;
        }

        //called from RoundManager
        async void SpawnEnemies()
        {
            //spawns amountToSpawn at spawnpoints, to add more spawn points add more under spawnPoints (unity), with a delay
            for (int i = 0; i < amountToSpawn; i++)
            {
                //for only one spawnPoint - spawns from itself :-)
                if (oneSpawn)
                {
                    Instantiate(enemy.gameObject, transform.position, transform.rotation, transform);
                }
                //for multiple spawnPoints - spawns from gameobjects that u pick :-)
                else
                {
                    CalculateSpawnPoint();
                    if (transform == null || currentSpawn == null) return;
                    Instantiate(enemy.gameObject, currentSpawn.position, currentSpawn.rotation, transform);
                }
                await Task.Delay(1000);
            }
            CalculateAmount();
        }

        void CalculateAmount()
        {
            amountToSpawn *= 2;
        }

        void CalculateSpawnPoint()
        {
            spawnPlace = Random.Range(0, spawnPoints.childCount);
            currentSpawn = spawnPoints.GetChild(spawnPlace);
        }
    }
}