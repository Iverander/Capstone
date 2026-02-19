using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Capstone
{
    public class Enemy : Creature
    {
        Player player;
        NavMeshAgent agent;

        int abilityToPreform;
        int coolDown;
        bool canAttack;

        [SerializeReference, SubclassSelector] public List<CombatAbility> abilities = new();


        void Start()
        {
            player = Player.instance;
            agent = GetComponent<NavMeshAgent>();
            
            foreach (var ability in abilities) //i forgot we have to initialize the abilities lol
            {
                if (ability == null) continue;
                ability.Initialize(this);
            }
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
            abilities[abilityToPreform].Perform<Player>();
        }
        
        
        private void OnDrawGizmosSelected() //Just to visualize the hitbox for the abilities
        {
            foreach (var ability in abilities)
            {
                if (ability == null) continue;
                if (!ability.ShowGizmos) continue;

                ability.Gizmos(transform);
            }
        }
    }
}
