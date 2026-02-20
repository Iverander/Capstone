using System.Threading.Tasks;
using UnityEngine;

namespace Capstone
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Enemy enemy;
        int amountToSpawn = 2;

        

        RoundManager roundManager;

        void Start()
        {
            roundManager = GetComponent<RoundManager>();
            roundManager.newRound.AddListener(SpawnEnemies);

        }

        async void SpawnEnemies()
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                await Task.Delay(1000);
                Instantiate(enemy.gameObject, transform.position, transform.rotation);
            }
            CalculateAmount();
        }

        void CalculateAmount()
        {
            amountToSpawn = amountToSpawn * 2;
        }
    }
}
