using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Capstone
{
    public class RoundManager : MonoBehaviour
    {
        /*
         * This script controls the rounds and spawning in the game.
         */

        [SerializeField] UIDocument UIObject;
        Label UIText;

        [ReadOnly]public int roundNr = 0;

        public static UnityEvent newRound = new();
        bool enemiesAlive;

        private void Start()
        {
            UIText = UIObject.rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);

            NewRound();
        }


        //Starts rounds + is the one they're subscribed to
        [ContextMenu("Start New Round")]
        public void NewRound()
        {
            roundNr++;
            //EnemySpawner, 
            newRound?.Invoke();
            StartCoroutine(UserInterface());
            Debug.Log("Starting round " + roundNr);
        }

        private void Update()
        {
            if (transform.childCount == 0) enemiesAlive = false;
            else enemiesAlive = true;

            if (!enemiesAlive) NewRound();
        }

        //Handles screenUI for game phases
        IEnumerator UserInterface()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            //Should update UI with text like roundNr Spawning enemies..
            UIText.text = "Starting round " + roundNr;
            yield return new WaitForSeconds(2.5f);
            UIText.style.visibility = new StyleEnum<Visibility> (Visibility.Hidden);
        }
    }
}