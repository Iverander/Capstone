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
        EnemyState enemyState;

        private bool firstAttack;

        [SerializeReference, SubclassSelector] public List<Ability> abilities = new(); //things has been simplified, you're welcome
        [SerializeField] Animator animator;

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

            RoundManager.UpdateEnemyCount(1);

            foreach (var ability in abilities) //i forgot we have to initialize the abilities lol
            {
                if (ability == null) continue;
                ability.Initialize(this);
            }
            agent.destination = player.transform.position;
            
            //chase
            enemyState = EnemyState.Chase;
        }

        // Update is called once per frame, unless it is called twice, but then something is wrong
        void Update()
        {
            
            if(!agent.enabled) return; //if the agent component is disabled, dont do shit thanks

            //Debug.Log(Vector3.Distance(player.transform.position, agent.transform.position));
            //Debug.Log (agent.destination);

            if (Vector3.Distance(player.transform.position, agent.transform.position) < 2) //selects the correct state
                enemyState = EnemyState.Attack;
            else
                enemyState = EnemyState.Chase;

            switch (enemyState)
            {
                case EnemyState.Chase:
                    animator.SetFloat("Speed", 1);
                    ChaseState();
                    break;
                case EnemyState.Attack:
                    animator.SetFloat("Speed", 0);
                    AttackState();
                    break;
            }
        }

        private void OnDestroy()
        {
            //when it fucking dies
            RoundManager.UpdateEnemyCount(-1);

        }
        //i remember time.Deltatime btw:)

        /// <summary>
        /// What to do when the enemy is in the attack state
        /// </summary>
        void AttackState()
        {
            agent.isStopped = true; //ensures that they wont move
            transform.LookAt(player.transform.position);
            
            if (abilities[abilityToPreform].onCooldown) return;
            StartCoroutine(Attack());
        }

        IEnumerator Attack()
        {
            if (firstAttack)
            {
                yield return new WaitForSeconds(abilities[abilityToPreform].cooldown / 2);
                firstAttack = false;

                if (enemyState != EnemyState.Attack || stunned)
                {
                    firstAttack = true;
                    yield break;   
                }
            }

            animator.SetTrigger("Punch");
            
            abilityToPreform = Random.Range(0, abilities.Count);
            abilities[abilityToPreform].Perform(); //preforms chosen ability, towards player creature (no longer towards a player creature as we use layers instead now like any good citizen)
            yield return new WaitForSeconds(abilities[abilityToPreform].cooldown);
        }

        /// <summary>
        /// What to do when the enemy is in the chase state
        /// </summary>
        void ChaseState()
        {
            //newPos
            agent.isStopped = false; //makes sure they can move
            firstAttack = true; //allows us to wait a little before a attack happens when it changes state
            NewDestination();
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

        //for stun & knockback
        public override IEnumerator Stun(float durationSeconds)
        {
            animator.SetBool("Stunned", true);
            agent.enabled = false;
            rb.isKinematic = false;
            stunned = true;
            stunEffect.SetActive(true);
            
            //rb.AddForce((transform.position - origin) * (knockback * 10), ForceMode.Force);

            yield return new WaitForSeconds(durationSeconds);
            
            stunned = false;
            rb.isKinematic = true;
            agent.enabled = true;
            stunEffect.SetActive(false);
            animator.SetBool("Stunned", false);
        }
    }
}