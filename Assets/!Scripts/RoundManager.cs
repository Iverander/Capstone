using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Capstone
{
    public class RoundManager : MonoBehaviour
    {
        /*
         * This script controls the rounds and spawning in the game.
         * TO mener det er tryggere å laste scenen på nytt, og legge inn dataene i playerprefs
         * Også er butikken også en egen scene
         * Evt. at når du lukker shoppen laster du gamescene, men lagre relevant data.
         */

        [SerializeField] UIDocument UIObject;
        Label UIText;
        Button roundButton;

        public static RoundManager instance; //makes it accessible from everywhere

        [ReadOnly] public int roundNr = 0;
        public static int round => instance.roundNr; //shortcut for the round number

        public static UnityEvent newRound = new();
        bool enemiesAlive;

        private void Start()
        {
            instance = this;
            UIText = UIObject.rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
            roundButton = UIObject.rootVisualElement.Q<Button>();
            roundButton.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);

            NewRound();
        }

        public void BetweenRounds()
        {
            roundNr++;
            StartCoroutine(UserInterfaceBetweenRounds());
        }


        //Starts rounds + is the one they're subscribed to
        [ContextMenu("Start New Round")]
        public void NewRound()
        {
            roundNr++;
            //EnemySpawner, 
            newRound?.Invoke();
            StartCoroutine(UserInterfaceNewRound());
            Debug.Log("Starting round " + roundNr);
        }

        private void Update()
        {
            if (transform.childCount == 0) enemiesAlive = false;
            else enemiesAlive = true;

            if (!enemiesAlive) BetweenRounds();

            //roundButton.clicked += () => NewRound();
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

        IEnumerator UserInterfaceBetweenRounds()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            //Should update UI with text like roundNr Spawning enemies..
            UIText.text = "Round " + (roundNr - 1);
            yield return new WaitForSeconds(2.5f);
            //UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }
    }
}