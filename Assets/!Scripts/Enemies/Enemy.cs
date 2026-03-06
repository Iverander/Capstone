using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace Capstone
{
    public enum EnemyState
    {
        Chase,
        Attack
    }

    public class Enemy : Creature
    {
        Player player;
        NavMeshAgent agent;

        int abilityToPreform;
        int coolDown;
        bool canAttack;
        EnemyState enemyState;

        [Expandable] public List<Ability> abilities = new(); //things has been simplified, you're welcome

        /*
         * State machine enum (thingy with chase and attack)
         * Depending on state, it acts differently, chase walks towards the player, attack chooses attack
         * AI should be predictable, default enemy walk towards you and try to punch you, other types of enemies can have different AI cus long range
         * Not every enemy will be in your face but this is like a zombie
         * To be fair I don't know how you wanna do it, whether or not you wanna emulate senses for the enemies or whatever - Torje
         */


        void Start()
        {
            player = Player.instance;
            agent = GetComponent<NavMeshAgent>();

            foreach (var ability in abilities) //i forgot we have to initialize the abilities lol
            {
                if (ability == null) continue;
                ability.Initialize(this);
            }
            agent.destination = player.transform.position;

            canAttack = true;
            //chase
            enemyState = EnemyState.Chase;
        }

        // Update is called once per frame, unless it is called twice, but then something is wrong
        void Update()
        {
            if(!agent.enabled) return; //if the agent component is disabled, dont do shit thanks

            //Debug.Log(Vector3.Distance(player.transform.position, agent.transform.position));
            //Debug.Log (agent.destination);

            switch (enemyState)
            {
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    StartCoroutine(Attack());
                    break;
            }
        }
        //i remember time.Deltatime btw:)

        IEnumerator Attack()
        {if (!canAttack) yield break;
            transform.LookAt(player.transform.position);

            canAttack = false;
            abilityToPreform = Random.Range(0, abilities.Count);
            abilities[abilityToPreform].Perform(); //preforms chosen ability, towards player creature
            //Debug.Log("attack happened");
            yield return new WaitForSeconds(5);
            canAttack = true;

            enemyState = EnemyState.Chase; //back to chase
        }

        void Chase()
        {
            //newPos

            if (Vector3.Distance(agent.destination, transform.position) < 2)
            {
                NewDestination();
            }
            agent.isStopped = false;

            //tryAttack
            if (Vector3.Distance(player.transform.position, agent.transform.position) < 2)
            {
                agent.isStopped = true;

                if (canAttack)
                {
                    enemyState = EnemyState.Attack;
                }
            }
        }

        private void NewDestination()
        {
            //Debug.Log(player.transform.position);
            agent.destination = player.transform.position;
        }


        //Just to visualize the hitbox for the abilities
        private void OnDrawGizmosSelected()
        {
            foreach (var ability in abilities)
            {
                if (ability == null) continue;
                if (!ability.ShowGizmos) continue;

                ability.Gizmos(transform);
            }
        }

        //for knockback
        public override async void Knockback(Vector3 origin, float knockback, float duration)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            knockbacked = true;
            
            rb.AddForce((transform.position - origin) * (knockback * 10), ForceMode.Force);

            await Task.Delay(Mathf.RoundToInt(1000 * duration));
            
            knockbacked = false;
            rb.isKinematic = true;
            agent.enabled = true;
        }
    }
}