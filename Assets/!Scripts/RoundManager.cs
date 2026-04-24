using NaughtyAttributes;
using System.Collections;
using Capstone.Datapoints;
using SceneSystem;
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
         * TO mener det er tryggere � laste scenen p� nytt, og legge inn dataene i playerprefs
         * Også er butikken ogs� en egen scene
         * Evt. at n�r du lukker shoppen laster du gamescene, men lagre relevant data.
         */

        public enum RoundState
        {
            None,
            DuringRound,
            BetweenRounds
        }

        public static RoundState roundState = RoundState.None;

        [SerializeField] UIDocument UIObject;
        Label UIText;

        public static RoundManager instance; //makes it accessible from everywhere

        [SerializeField] float firstRoundDelaySeconds = 3;

        [ReadOnly] public int roundNr = 0;
        public static int round => instance.roundNr; //shortcut for the round number

        public static UnityEvent onNewRound = new();
        public static UnityEvent onBetweenRound = new();
        public int enemiesAlive;
        
        [SerializeField] Scene hlslScene;
        [SerializeField] Scene sgScene;

        public static int highestEnemyCount { get; private set; }
 

        private IEnumerator Start()
        {
            instance = this;
            UIText = UIObject.rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
            
            DataManager.instance.StartNewSession();

            yield return new WaitForSeconds(firstRoundDelaySeconds);

            NewRound();
        }
        void OnDestroy()
        {
            roundState = RoundState.None;
        }

        public void BetweenRounds()
        {
            Session.active.NewSection($"Round End");
            onBetweenRound?.Invoke();
            roundState = RoundState.BetweenRounds;
        }


        //Starts rounds + is the one they're subscribed to
        [ContextMenu("Start New Round")]
        public void NewRound()
        {
            highestEnemyCount = 0;
            roundState = RoundState.DuringRound;
            roundNr++;
            //EnemySpawner, 
            onNewRound?.Invoke();
            StartCoroutine(UserInterfaceNewRound());
            Debug.Log("Starting round " + roundNr);
            
            if (Settings.active.shaderType == ShaderType.HLSL)
            {
                Settings.active.shaderType = ShaderType.ShaderGraph;
                hlslScene.Unload();
                sgScene.Load();
            }
            else
            {
                Settings.active.shaderType = ShaderType.HLSL;
                sgScene.Unload();
                hlslScene.Load();
            }
 
        }


        //Handles screenUI for game phases
        IEnumerator UserInterfaceNewRound()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            //Should update UI with text like roundNr Spawning enemies..
            UIText.text = "Starting round " + (roundNr);
            yield return new WaitForSeconds(3f);
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }

        public static void UpdateEnemyCount(int amount)
        {
            instance.enemiesAlive += amount;

            if(instance.enemiesAlive > highestEnemyCount)
                highestEnemyCount = instance.enemiesAlive;

            if(instance.enemiesAlive <= 0)
            {
                instance.BetweenRounds();
            }
        }
    }
}