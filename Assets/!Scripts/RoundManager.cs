using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Capstone
{

    [DefaultExecutionOrder(100)]
    public class RoundManager : MonoBehaviour //this is actually the gamemanager hidden under a secret name...
    {
        /*
         * This script controls the rounds and spawning in the game.
         * TO mener det er tryggere å laste scenen på nytt, og legge inn dataene i playerprefs
         * Også er butikken også en egen scene
         * Evt. at når du lukker shoppen laster du gamescene, men lagre relevant data.
         */

        public enum RoundState
        {
            DuringRound,
            BetweenRounds
        }

        public RoundState roundState;

        [SerializeField] UIDocument UIObject;
        Label UIText;

        public static RoundManager instance; //makes it accessible from everywhere

        [ReadOnly] public int roundNr = 0;
        public static int round => instance.roundNr; //shortcut for the round number

        public static UnityEvent newRound = new();
        public static UnityEvent betweenRound = new();
        public int enemiesAlive;
 

        private void Start()
        {
            instance = this;
            UIText = UIObject.rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);

            NewRound();
        }

        public void BetweenRounds()
        {
            betweenRound?.Invoke();
            roundState = RoundState.BetweenRounds;
        }


        //Starts rounds + is the one they're subscribed to
        [ContextMenu("Start New Round")]
        public void NewRound()
        {
            roundState = RoundState.DuringRound;
            roundNr++;
            //EnemySpawner, 
            newRound?.Invoke();
            StartCoroutine(UserInterfaceNewRound());
            Debug.Log("Starting round " + roundNr);
        }


        //Handles screenUI for game phases
        IEnumerator UserInterfaceNewRound()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            //Should update UI with text like roundNr Spawning enemies..
            UIText.text = "Starting round " + (roundNr - 1);
            yield return new WaitForSeconds(2.5f);
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }

        public static void UpdateEnemyCount(int amount)
        {
            instance.enemiesAlive += amount;
            if(instance.enemiesAlive <= 0)
            {
                instance.BetweenRounds();
            }
        }
    }
}