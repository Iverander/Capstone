using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Capstone
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Enemy enemy;
        [SerializeField] int amountToSpawn = 2;

        void Start()
        {
            RoundManager.newRound.AddListener(SpawnEnemies);
        }

        async void SpawnEnemies()
        {
            Debug.Log("whatever the fuvk");
            for (int i = 0; i < amountToSpawn; i++)
            {
                Instantiate(enemy.gameObject, transform.position, transform.rotation, transform);
                await Task.Delay(1000);
            }
            CalculateAmount();
        }

        void CalculateAmount()
        {
            amountToSpawn *= 2;
        }
    }
}
