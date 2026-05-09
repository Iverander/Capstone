using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class NewRoundTrigger : MonoBehaviour
    {
        [SerializeField] private UIDocument UIObject;
        private float countdown;
        private readonly float timeToWait = 3;
        private Label UIText;

        private void Start()
        {
            RoundManager.onBetweenRound.AddListener(Enable);
            RoundManager.onNewRound.AddListener(Disable);

            UIText = UIObject.rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);

            Disable();
        }

        private void OnTriggerExit(Collider other)
        {
            countdown = timeToWait;
            endCountDownUI();
        }

        private void OnTriggerStay(Collider other)
        {
            if (RoundManager.roundState == RoundManager.RoundState.DuringRound) return;
            countdown -= Time.deltaTime;
            CountDownUI();

            if (countdown <= 0)
            {
                Debug.Log("five seconds have passed");
                RoundManager.instance.NewRound();
            }
            //countdown = 0;
        }

        private void Enable()
        {
            gameObject.SetActive(true);
            countdown = timeToWait;
        }

        private void Disable()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
            gameObject.SetActive(false);
        }

        private void CountDownUI()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            UIText.text = countdown.ToString("0.0");
            //Should update UI with text like roundNr Spawning enemies..
        }

        private void endCountDownUI()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }
    }
}