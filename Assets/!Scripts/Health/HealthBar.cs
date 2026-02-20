using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health;
        
        ProgressBar progressBar;

        private void Start()
        {
            health.Damaged.AddListener(UpdateProgress);
            progressBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>();
            progressBar.value = health.health;
        }
        
        ///Updates the progress:)
        public void UpdateProgress(float health)
        {
            progressBar.value = health;
        }

    }
}
