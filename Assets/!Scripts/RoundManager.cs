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
         */

        Label UIText;
        UIDocument UIDoc;

        public int roundNr = 0;

        [HideInInspector] public UnityEvent newRound;

        [SerializeField] bool nextRound;

        private void Start()
        {
            UIText = GetComponent<UIDocument>().rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }

        [ContextMenu("Start New Round")]
        public void NewRound()
        {
            roundNr++;
            StartCoroutine(UserInterface());
            Debug.Log("Starting round " + roundNr);
            newRound?.Invoke();
        }


  
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