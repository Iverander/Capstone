using System.Collections;
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
            canAttack = true;
            
            foreach (var ability in abilities) //i forgot we have to initialize the abilities lol
            {
                if (ability == null) continue;
                ability.Initialize(this);
            }
            agent.destination = player.transform.position;
        }

        // Update is called once per frame
        void Update()
        {

            //Debug.Log(Vector3.Distance(player.transform.position, agent.transform.position));

            //Debug.Log (agent.destination);



            if (Vector3.Distance(player.transform.position, agent.transform.position) < 2)
            {
                agent.isStopped = true;
                if (canAttack)
                {
                    transform.LookAt(player.transform.position);
                    StartCoroutine(Attack());
                    Debug.Log("Attacking");
                }
            }

            else
            {
                if (Vector3.Distance(agent.destination, transform.position) < 2)
                {
                    NewDestination();
                }
                agent.isStopped = false;
            }
        }

        IEnumerator Attack()
        {
            canAttack = false;
            abilityToPreform = Random.Range(0, abilities.Count);
            abilities[abilityToPreform].Perform<Player>();
            Debug.Log("attack happened");
            yield return new WaitForSeconds(5);
            canAttack = true;
        }

        private void NewDestination()
        {
            Debug.Log(player.transform.position);
            agent.destination = player.transform.position;
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