using UnityEngine;
using UnityEngine.AI;

namespace Capstone
{
    public class Enemy : MonoBehaviour
    {
        Player player;
        NavMeshAgent agent;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = Player.instance;
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            agent.nextPosition = player.transform.position;
        }
    }
}
