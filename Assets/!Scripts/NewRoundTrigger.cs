using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class NewRoundTrigger : MonoBehaviour
    {
        float countdown;
        [SerializeField] float timeToWait = 2;

        void Start()
        {
            RoundManager.onBetweenRound.AddListener(Enable);
            RoundManager.onNewRound.AddListener(Disable);
            Disable();
        }

        void Enable()
        {
            gameObject.SetActive(true);
        }
        void Disable()
        {
            gameObject.SetActive(false);
        }

        [SerializeField] UIDocument UIObject;
        Label UIText;

        private void Start()
        {
            UIText = UIObject.rootVisualElement.Q<Label>();
            UIText.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }

        private void OnTriggerStay(Collider other)
        {
            if (RoundManager.instance.roundState == RoundManager.RoundState.DuringRound)
            {
                return;
            }
            countdown += Time.deltaTime;
            if (countdown >= timeToWait)
            {
                Debug.Log("five seconds have passed");
                RoundManager.instance.NewRound();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            countdown = 0;
        }
    }
}
