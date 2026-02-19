using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Capstone
{
    public class Enemy : MonoBehaviour
    {
        Player player;
        NavMeshAgent agent;

        int abilityToPreform;
        int coolDown;
        bool canAttack;

        [SerializeReference, SubclassSelector] public List<CombatAbility> abilities = new List<CombatAbility>();


        void Start()
        {
            player = Player.instance;
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            agent.destination = player.transform.position;

            if (Vector3.Distance(player.transform.position, agent.transform.position) < 1)
            {
                Attack();
            }
        }

        void Attack()
        {
            abilityToPreform = Random.Range(0, abilities.Count);
            abilities[abilityToPreform].Preform();
        }
    }
}
