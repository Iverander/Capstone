using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class NewRoundTrigger : MonoBehaviour
    {
        float countdown;
        float timeToWait = 3;

        [SerializeField] UIDocument UIObject;
        Label UIText;

        void Start()
        {
            RoundManager.onBetweenRound.AddListener(Enable);
            RoundManager.onNewRound.AddListener(Disable);

            UIText = UIObject.rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);

            Disable();
        }

        void Enable()
        {
            gameObject.SetActive(true);
            countdown = timeToWait;
        }
        void Disable()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
            gameObject.SetActive(false);
        }

        private void OnTriggerStay(Collider other)
        {
            if (RoundManager.instance.roundState == RoundManager.RoundState.DuringRound)
            {
                return;
            }
            countdown -= Time.deltaTime;
            CountDownUI();

            if (countdown <= 0)
            {
                Debug.Log("five seconds have passed");
                RoundManager.instance.NewRound();
            }
            //countdown = 0;
        }

        private void OnTriggerExit(Collider other)
        {
            countdown = timeToWait;
            endCountDownUI();
        }

        void CountDownUI()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            UIText.text = countdown.ToString("0.0");
            //Should update UI with text like roundNr Spawning enemies..
        }

        void endCountDownUI()
        {
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }
    }
}
